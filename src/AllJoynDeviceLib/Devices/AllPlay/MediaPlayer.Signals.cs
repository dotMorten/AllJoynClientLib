using System;
using System.Collections.Generic;
using System.Linq;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    public partial class MediaPlayer
    {
        private ISignal _playStateChangedSignal;
        private ISignal _playlistChanged;
        private ISignal _shuffleModeChanged;
        private ISignal _enabledControlsChanged;
        private ISignal _loopModeChanged;
        private ISignal _endOfPlaybackChanged;
        private ISignal _interruptibleChanged;
        private ISignal _onPlaybackErrorChanged;

        private void InitSignals()
        {
            _playStateChangedSignal = mediaPlayer.GetSignal(nameof(PlayStateChanged));
            _playStateChangedSignal.SignalRaised += StateChangedSignal_SignalRaised;
            _playlistChanged = mediaPlayer.GetSignal(nameof(PlaylistChanged));
            _playlistChanged.SignalRaised += PlaylistChanged_SignalRaised;
            _shuffleModeChanged = mediaPlayer.GetSignal(nameof(ShuffleModeChanged));
            _shuffleModeChanged.SignalRaised += ShuffleModeChanged_SignalRaised;
            _enabledControlsChanged = mediaPlayer.GetSignal(nameof(EnabledControlsChanged));
            _enabledControlsChanged.SignalRaised += EnabledControlsChanged_SignalRaised;
            _loopModeChanged = mediaPlayer.GetSignal(nameof(LoopModeChanged));
            _loopModeChanged.SignalRaised += LoopModeChanged_SignalRaised;
            _endOfPlaybackChanged = mediaPlayer.GetSignal(nameof(EndOfPlayback));
            _endOfPlaybackChanged.SignalRaised += EndOfPlaybackChanged_SignalRaised;
            _interruptibleChanged = mediaPlayer.GetSignal(nameof(InterruptibleChanged));
            _interruptibleChanged.SignalRaised += InterruptibleChanged_SignalRaised;
            _onPlaybackErrorChanged = mediaPlayer.GetSignal(nameof(OnPlaybackError));
            _onPlaybackErrorChanged.SignalRaised += OnPlaybackErrorChanged_SignalRaised;
        }

        internal void OnDeviceLost()
        {
            _playStateChangedSignal.SignalRaised -= StateChangedSignal_SignalRaised;
            _playlistChanged.SignalRaised -= PlaylistChanged_SignalRaised;
            _shuffleModeChanged.SignalRaised -= ShuffleModeChanged_SignalRaised;
            _enabledControlsChanged.SignalRaised -= EnabledControlsChanged_SignalRaised;
            _loopModeChanged.SignalRaised -= LoopModeChanged_SignalRaised;
            _endOfPlaybackChanged.SignalRaised -= EndOfPlaybackChanged_SignalRaised;
            _interruptibleChanged.SignalRaised -= InterruptibleChanged_SignalRaised;
            _onPlaybackErrorChanged.SignalRaised -= OnPlaybackErrorChanged_SignalRaised;
        }

        private void EndOfPlaybackChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            EndOfPlayback?.Invoke(this, EventArgs.Empty);
        }

        private void InterruptibleChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            InterruptibleChanged?.Invoke(this, (bool)args[0]);
        }

        private void ShuffleModeChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            ShuffleModeChanged?.Invoke(this, StringToShuffleMode(args.First() as string));
        }

        private void LoopModeChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            LoopModeChanged?.Invoke(this, StringToLoopMode(args.First() as string));
        }

        private void EnabledControlsChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            var ctrls = args.First();
            EnabledControlsChanged?.Invoke(this, new EnabledControls(ctrls as IList<KeyValuePair<object, object>>));
        }

        private void PlaylistChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            PlaylistChanged?.Invoke(this, EventArgs.Empty);
        }

        private void StateChangedSignal_SignalRaised(ISignal sender, IList<object> args)
        {
            // <signal name="PlayStateChanged">
            //     <arg name="state" type="(sxuuuiia(ssssxsssa{ss}a{sv}v))" direction="out"/>
            // </signal>
            var state = args.First() as AllJoynMessageArgStructure;
            PlayStateChanged?.Invoke(this, new PlayState(state));
        }

        private void OnPlaybackErrorChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            OnPlaybackError?.Invoke(this, new PlaybackErrorEventArgs(args));
        }

        /// <summary>
        /// Raised when the playback has ended
        /// </summary>
        public event EventHandler EndOfPlayback;

        /// <summary>
        /// Raised when the set of enavled controls has changed
        /// </summary>
        public event EventHandler<EnabledControls> EnabledControlsChanged;

        /// <summary>
        /// Raised when the interruptible state has changed
        /// </summary>
        public event EventHandler<bool> InterruptibleChanged;

        /// <summary>
        /// Raised when the loop mode has changed
        /// </summary>
        public event EventHandler<LoopMode> LoopModeChanged;

        /// <summary>
        /// Raised when the play state has changed
        /// </summary>
        public event EventHandler<PlayState> PlayStateChanged;

        /// <summary>
        /// Raised when the playlist has changed
        /// </summary>
        public event EventHandler PlaylistChanged;

        /// <summary>
        /// Raised when the shuffle mode has changed
        /// </summary>
        public event EventHandler<ShuffleMode> ShuffleModeChanged;

        /// <summary>
        /// Raised if a playback error occured
        /// </summary>
        public event EventHandler<PlaybackErrorEventArgs> OnPlaybackError;
    }
}
