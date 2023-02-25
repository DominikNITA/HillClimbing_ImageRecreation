using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using FFMpegCore.Extensions.System.Drawing.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class VideoGenerator
    {
        public VideoGenerator()
        {
            GlobalFFOptions.Configure(new FFOptions { BinaryFolder = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/FFMPEG" });
        }

        public async void Test()
        {
            var files = System.IO.Directory.GetFiles($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/FFMPEG");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            IEnumerable<IVideoFrame> CreateFrames(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var bitmap = new Bitmap("E:\\Programming\\HillClimbingAlgorithm\\HillClimbing_ImageRecreation\\Development\\638129245868643809bb279d37-de37-4196-8932-01fde27e61ff\\10.png");
                    yield return new BitmapVideoFrameWrapper(bitmap); //method that generates of receives the next frame
                }
            }

            var videoFramesSource = new RawVideoPipeSource(CreateFrames(64))
            {
                FrameRate = 8 //set source frame rate
            };

            await FFMpegArguments
            .FromPipeInput(videoFramesSource)
            .OutputToFile("T:\\HCA\\test.mp4", true)
            .ProcessAsynchronously();

            FFMpeg.JoinImageSequence(@"T:\HCA\joined_video.mp4", frameRate: 1,
                "E:\\Programming\\HillClimbingAlgorithm\\HillClimbing_ImageRecreation\\Development\\638129245868643809bb279d37-de37-4196-8932-01fde27e61ff\\1.png",
                "E:\\Programming\\HillClimbingAlgorithm\\HillClimbing_ImageRecreation\\Development\\638129245868643809bb279d37-de37-4196-8932-01fde27e61ff\\10.png",
                "E:\\Programming\\HillClimbingAlgorithm\\HillClimbing_ImageRecreation\\Development\\638129245868643809bb279d37-de37-4196-8932-01fde27e61ff\\20.png"
);
        }
    }
}
