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
using Logic.Helpers;

namespace Logic
{
    //TODO: Add parameter for FFMPEG binaries location
    public class VideoGenerator
    {
        public VideoGenerator()
        {
            GlobalFFOptions.Configure(new FFOptions { BinaryFolder = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/FFMPEG" });
        }

        public async Task<string> GenerateTimelapseVideo(string? id)
        {
            var images = Directory.GetFiles(StorageHelper.GetPathForIterationsFolderById(id));
            foreach (var file in images)
            {
                Console.WriteLine(file);
            }

            IEnumerable<IVideoFrame> CreateFrames(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var bitmap = new Bitmap(images[i]);
                    yield return new BitmapVideoFrameWrapper(bitmap); //method that generates the next frame
                }
            }

            var videoFramesSource = new RawVideoPipeSource(CreateFrames(images.Length))
            {
                FrameRate = 10 // add as parameter
            };

            await FFMpegArguments
            .FromPipeInput(videoFramesSource)
            .OutputToFile(StorageHelper.GetPathForTimelapseFile(id), true)
            .ProcessAsynchronously();

            return StorageHelper.GetPathForTimelapseFile(id);
            //FFMpeg.JoinImageSequence(StorageHelper.GetPathForTimelapseFile(id) + "sq", frameRate: 2,
            //    images);
        }
    }
}
