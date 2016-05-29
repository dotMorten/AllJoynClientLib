using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _playlistChanged.SignalRaised += _playlistChanged_SignalRaised;
            _shuffleModeChanged = mediaPlayer.GetSignal(nameof(ShuffleModeChanged));
            _shuffleModeChanged.SignalRaised += _shuffleModeChanged_SignalRaised;
            _enabledControlsChanged = mediaPlayer.GetSignal(nameof(EnabledControlsChanged));
            _enabledControlsChanged.SignalRaised += _enabledControlsChanged_SignalRaised;
            _loopModeChanged = mediaPlayer.GetSignal(nameof(LoopModeChanged));
            _loopModeChanged.SignalRaised += _loopModeChanged_SignalRaised;
            _endOfPlaybackChanged = mediaPlayer.GetSignal(nameof(EndOfPlayback));
            _endOfPlaybackChanged.SignalRaised += _endOfPlaybackChanged_SignalRaised;
            _interruptibleChanged = mediaPlayer.GetSignal(nameof(InterruptibleChanged));
            _interruptibleChanged.SignalRaised += _interruptibleChanged_SignalRaised;
            _onPlaybackErrorChanged = mediaPlayer.GetSignal(nameof(OnPlaybackError));
            _onPlaybackErrorChanged.SignalRaised += _onPlaybackErrorChanged_SignalRaised;
        }

        private void _endOfPlaybackChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            EndOfPlayback?.Invoke(this, EventArgs.Empty);
        }

        private void _interruptibleChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            InterruptibleChanged?.Invoke(this, (bool)args[0]);
        }

        private void _shuffleModeChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            ShuffleModeChanged?.Invoke(this, StringToShuffleMode(args.First() as string));
        }

        private void _loopModeChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            LoopModeChanged?.Invoke(this, StringToLoopMode(args.First() as string));
        }
        
        private void _enabledControlsChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            var ctrls = args.First();
            EnabledControlsChanged?.Invoke(this, new EnabledControls(ctrls as IList<KeyValuePair<object, object>>));
        }

        private void _playlistChanged_SignalRaised(ISignal sender, IList<object> args)
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

        private void _onPlaybackErrorChanged_SignalRaised(ISignal sender, IList<object> args)
        {
            OnPlaybackError?.Invoke(this, new PlaybackErrorEventArgs(args)); 
        }


        public event EventHandler EndOfPlayback;
        public event EventHandler<EnabledControls> EnabledControlsChanged;
        public event EventHandler<bool> InterruptibleChanged;
        public event EventHandler<LoopMode> LoopModeChanged;
        public event EventHandler<PlayState> PlayStateChanged;
        public event EventHandler PlaylistChanged;
        public event EventHandler<ShuffleMode> ShuffleModeChanged;
        public event EventHandler<PlaybackErrorEventArgs> OnPlaybackError;
    }
    public class PlaybackErrorEventArgs : EventArgs
    {
        internal PlaybackErrorEventArgs(IList<object> args)
        {
            Index = (int)args[0];
            Error = args[1] as string;
            Description = args[2] as string;
        }
        public int Index { get; }
        public string Error { get; }
        public string Description { get; }
    }
}
