using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Serviecs
{
    public class MediaStorage : IMediaStorage
    {
        public async Task<string> SaveImageAsync(string tempPath, string fullName)
        {
            var mainSpace = GenerateMediaSpace();
            var spaceName = GeneratePersonSpace(mainSpace, fullName);
            var photo = Path.Combine(spaceName, $"{fullName.Replace(" ", "")}Photo{Path.GetExtension(tempPath)}");
            File.Copy(tempPath, photo, true);
            var relativePath = photo.Replace(mainSpace, "Media");
            File.Delete(tempPath);
            return relativePath;
        }

        private string GenerateMediaSpace()
        {
            var thisPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            var space = Path.Combine(thisPath, "Media");
            if(!Directory.Exists(space))
                Directory.CreateDirectory(space);
            return space;
        }

        private string GeneratePersonSpace(string mainSpace, string fullName)
        {
            var personSpace = Path.Combine(
                mainSpace,
                string.Join("", fullName
                .Split(' ')
                .Select(s => s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1).ToLower())
                .ToList()));
            if (!Directory.Exists(personSpace))
                Directory.CreateDirectory(personSpace);

            return personSpace;
        }
    }
}
