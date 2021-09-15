using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace PervySageBot.ContentFetcher.ContentManipulation
{
    public static class ContentIO
    {
        public const string FilePrefix = "tempPervySageBotAsset";
        public static string[] SaveDataStreamImg(Stream[] streams)
        {
            string[] FileLocations = new string[streams.Length];
            for (int i = 0; i < streams.Length; i++)
            {
                var image = Bitmap.FromStream(streams[i]);
                image.Save($"{FilePrefix}{i}.jpeg");
                FileLocations[i] = $"{FilePrefix}{i}.jpeg";
            }
            return FileLocations;
        }
        public static Dictionary<string,Stream> GetDataStreams(Stream[] streams)
        {
            Dictionary<string, Stream> ImgStreams = new Dictionary<string, Stream>();
            int index = 0;
            foreach (var imgStream in streams)
            {
                ImgStreams.Add($"{FilePrefix}{index}.jpeg", imgStream);
                index++;
            }
            return ImgStreams;
        }

        public static void DeleteContent(int numberOfFilesToBeDeleted)
        {

            for (int i = 0; i < numberOfFilesToBeDeleted; i++)
            {
                if (File.Exists($"pic{i}.jpeg"))
                {
                    File.Delete($"pic{i}.jpeg");
                }
            }
        }
    }
}
