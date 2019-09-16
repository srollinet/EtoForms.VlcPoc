using Eto;
using Eto.Forms;
using LibVLCSharp.Shared;

namespace EtoForms.VlcPoc.Controls
{
    [Handler(typeof(IVideoView))]
    public class VideoView : Control
    {
        new IVideoView Handler => (IVideoView)base.Handler;

        public MediaPlayer MediaPlayer
        {
            get => Handler.MediaPlayer;
            set => Handler.MediaPlayer = value;
        }

        // interface to the platform implementations
        public interface IVideoView : IHandler
        {
            MediaPlayer MediaPlayer { get; set; }
        }
    }
}