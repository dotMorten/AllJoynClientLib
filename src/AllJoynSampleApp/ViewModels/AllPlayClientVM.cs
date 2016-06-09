using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AllJoynClientLib.Devices.AllPlay;
using AllJoynClientLib.Devices;

namespace AllJoynSampleApp.ViewModels
{
    public class MediaPlayerVM : ViewModelBase
    {
        private readonly AllPlayClient _client;
        private PlayState lastKnownState;
        private DateTime timeOfLastKnownState;
        private EnabledControls _enabledControls;
        private readonly Windows.UI.Xaml.DispatcherTimer positionTimer;
        private PlayerInfo playerInfo;

        public MediaPlayerVM(AllPlayClient client)
        {
            _client = client;
            positionTimer = new Windows.UI.Xaml.DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            positionTimer.Tick += PositionTimer_Tick;
            client.MediaPlayer.PlayStateChanged += PlayStateChanged;
            client.MediaPlayer.PlaylistChanged += MediaPlayer_PlaylistChanged;
            client.MediaPlayer.ShuffleModeChanged += MediaPlayer_ShuffleModeChanged;
            client.MediaPlayer.LoopModeChanged += MediaPlayer_LoopModeChanged;
            client.MediaPlayer.InterruptibleChanged += MediaPlayer_InterruptibleChanged;
            client.MediaPlayer.EnabledControlsChanged += MediaPlayer_EnabledControlsChanged;
            client.MediaPlayer.EndOfPlayback += MediaPlayer_EndOfPlayback;
            client.MediaPlayer.GetPlayerInfoAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) playerInfo = state.Result;
            });

            client.MediaPlayer.GetPlayStateAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) UpdateMediaState(state.Result);
            });
            client.MediaPlayer.GetEnabledControlsAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) UpdateEnabledControls(state.Result);
            });
            client.MediaPlayer.GetPlaylistAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) UpdatePlaylist(state.Result);
            });
            client.MediaPlayer.GetLoopModeAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) { LoopMode = state.Result;
                    OnPropertyChanged(nameof(LoopMode)); };
            });
            client.MediaPlayer.GetShuffleModeAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) { ShuffleMode = state.Result;
                    OnPropertyChanged(nameof(ShuffleMode)); };
            });
            client.MediaPlayer.GetInterruptibleAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) { IsInterruptible = state.Result;
                    OnPropertyChanged(nameof(IsInterruptible)); };
            });
            if (client.Volume != null)
            {
                client.Volume.VolumeChanged += Volume_VolumeChanged;
                client.Volume.MuteChanged += Volume_MuteChanged;
                client.Volume.EnabledChanged += Volume_EnabledChanged;
                client.Volume.GetVolumeAsync().ContinueWith((state) =>
                {
                    if (!state.IsFaulted && !state.IsCanceled) { Volume = state.Result;
                        OnPropertyChanged(nameof(Volume)); };
                });
                client.Volume.GetVolumeEnabledAsync().ContinueWith((state) =>
                {
                    if (!state.IsFaulted && !state.IsCanceled) { Volume_EnabledChanged(client.Volume, state.Result); };
                });
                client.Volume.GetMuteAsync().ContinueWith((state) =>
                {
                    if (!state.IsFaulted && !state.IsCanceled) { Volume_MuteChanged(client.Volume, state.Result); };
                });
            }

            nextCommand = new GenericCommand((o) => { _client.MediaPlayer.NextAsync(); }, (o) => { return _enabledControls == null ? false : _enabledControls.Next; });
            previousCommand = new GenericCommand((o) => { _client.MediaPlayer.PreviousAsync(); }, (o) => { return _enabledControls == null ? false : _enabledControls.Previous; });
            stopCommand = new GenericCommand((o) => { _client.MediaPlayer.StopAsync(); });
            resumeCommand = new GenericCommand((o) => { _client.MediaPlayer.ResumeAsync(); });

            volumeUpCommand = new GenericCommand((o) => { _client.Volume.AdjustVolumeAsync(2); }, (o) => { return IsVolumeEnabled; });
            volumeDownCommand = new GenericCommand((o) => { _client.Volume.AdjustVolumeAsync(-2); }, (o) => { return IsVolumeEnabled; });
        }

        public AllPlayClient Client { get { return _client; } }

        private void PositionTimer_Tick(object sender, object e)
        {
            OnPropertyChanged(nameof(Position));
        }
        
        public TimeSpan Position
        {
            get
            {
                int seconds = 0;
                if (lastKnownState != null)
                {
                    //Round to whole seconds - prettier in the view
                    if (lastKnownState.State == MediaState.Playing)
                    {
                        seconds = (int)(DateTime.Now - timeOfLastKnownState + lastKnownState.Position).TotalSeconds;
                    }
                    else
                        seconds = (int)lastKnownState.Position.TotalSeconds;
                }
                return TimeSpan.FromSeconds(seconds);
            }
        }
        public Media CurrentMedia { get { return lastKnownState?.CurrentMedia; } }
        public Media NextMedia { get { return lastKnownState?.NextMedia; } }
        public MediaState MediaState { get { return lastKnownState == null ? MediaState.Stopped : lastKnownState.State; } }

        public LoopMode LoopMode { get; private set; }
        public ShuffleMode ShuffleMode { get; private set; }
        public Playlist Playlist { get; private set; }

        private readonly GenericCommand nextCommand;
        public ICommand NextCommand { get { return nextCommand; } }
        private readonly GenericCommand previousCommand;
        public ICommand PreviousCommand { get { return previousCommand; } }
        private readonly GenericCommand stopCommand;
        public ICommand StopCommand { get { return stopCommand; } }
        private readonly GenericCommand resumeCommand;
        public ICommand ResumeCommand { get { return resumeCommand; } }

        private void MediaPlayer_EndOfPlayback(object sender, EventArgs e)
        {
        }

        private void MediaPlayer_EnabledControlsChanged(object sender, EnabledControls e)
        {
            UpdateEnabledControls(e);
        }

        private void UpdateEnabledControls(EnabledControls controls)
        {
            var old = _enabledControls;
            _enabledControls = controls;
            if (old == null || old.Next != controls.Next)
                nextCommand.RaiseCanExecuteChanged();
            if (old == null || old.Previous != controls.Previous)
                previousCommand.RaiseCanExecuteChanged();
        }

        public bool IsInterruptible { get; private set; } = true;

        private void MediaPlayer_InterruptibleChanged(object sender, bool interruptible)
        {
            //If a track can't be skipped/fastforwarded
            IsInterruptible = interruptible;
            OnPropertyChanged(nameof(interruptible));
        }

        public void PlayListEntry(int index)
        {
            _client.MediaPlayer.PlayAsync(index, TimeSpan.Zero, false);
        }

        private void MediaPlayer_LoopModeChanged(object sender, LoopMode e)
        {
            LoopMode = e;
            OnPropertyChanged(nameof(LoopMode));
        }

        private void MediaPlayer_ShuffleModeChanged(object sender, ShuffleMode e)
        {
            ShuffleMode = e;
            OnPropertyChanged(nameof(ShuffleMode));
        }

        private void MediaPlayer_PlaylistChanged(object sender, EventArgs e)
        {
            _client.MediaPlayer.GetPlaylistAsync().ContinueWith((state) =>
            {
                if (!state.IsFaulted && !state.IsCanceled) UpdatePlaylist(state.Result);
            });
        }

        private void UpdatePlaylist(Playlist result)
        {
            Playlist = result;
            OnPropertyChanged(nameof(Playlist));
        }

        private void PlayStateChanged(object sender, PlayState e)
        {
            UpdateMediaState(e);
        }

        private void UpdateMediaState(PlayState state)
        {
            lastKnownState = state;
            timeOfLastKnownState = DateTime.Now;
            var _= Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (state.State == MediaState.Playing)
                {
                    if (!positionTimer.IsEnabled)
                        positionTimer.Start();
                }
                else
                {
                    positionTimer.Stop();
                }
            });
            OnPropertyChanged(nameof(CurrentMedia), nameof(NextMedia), nameof(MediaState), nameof(Position));
        }

        #region Volume
        public bool IsVolumeEnabled { get; private set; }
        public bool IsMuted { get; private set; }
        public Int16 Volume { get; private set; }

        private void Volume_VolumeChanged(object sender, Int16 volume)
        {
            Volume = volume;
            OnPropertyChanged(nameof(Volume));
        }

        private void Volume_EnabledChanged(object sender, bool e)
        {
            IsVolumeEnabled = e;
            var _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                OnPropertyChanged(nameof(IsVolumeEnabled));
                volumeUpCommand.RaiseCanExecuteChanged();
                volumeDownCommand.RaiseCanExecuteChanged();
            });
        }

        private void Volume_MuteChanged(object sender, bool e)
        {
            IsMuted = e;
            OnPropertyChanged(nameof(IsMuted));
        }

        private readonly GenericCommand volumeDownCommand;
        public ICommand VolumeDownCommand { get { return volumeDownCommand; } }
        private readonly GenericCommand volumeUpCommand;
        public ICommand VolumeUpCommand { get { return volumeUpCommand; } }
        #endregion
    }
}
