using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.VlcPoc.Controls;
using LibVLCSharp.Shared;

namespace EtoForms.VlcPoc
{
    public class MainForm : Form
    {
        public MainForm()
        {
            ClientSize = new Size(400, 350);

            var videoView = new VideoView();
            Content = videoView;
            
            var libVlc = new LibVLC();
            var mp = new MediaPlayer(libVlc);
            videoView.MediaPlayer = mp;
            
            var media = new Media(libVlc, "/home/sro/Downloads/jellyfish-3-mbps-hd-h264.mkv");
            videoView.Ready += (sender, args) => mp.Play(media);
        }
    }
}