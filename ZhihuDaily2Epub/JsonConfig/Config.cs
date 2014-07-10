//
// Copyright (C) 2012 Timo Dörr
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZhihuDaily2Epub.JsonConfig{
    public static class Config{
        public static dynamic Default = new ConfigObject();

        public static string defaultEnding = ".conf";


        static Config() {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var dir = Path.GetDirectoryName(filePath);
            string basePath = Path.Combine(dir, "Config\\"); 
            Default = ApplyFromDirectory(basePath);
        }

        public static ConfigObject ApplyJsonFromFileInfo(FileInfo file, ConfigObject config = null) {
            string overlay_json = File.ReadAllText(file.FullName);
            dynamic overlay_config = ParseJson(overlay_json);
            return Merger.Merge(overlay_config, config);
        }

        public static ConfigObject ApplyJsonFromPath(string path, ConfigObject config = null) {
            return ApplyJsonFromFileInfo(new FileInfo(path), config);
        }

        public static ConfigObject ApplyJson(string json, ConfigObject config = null) {
            if (config == null) {
                config = new ConfigObject();
            }

            dynamic parsed = ParseJson(json);
            return Merger.Merge(parsed, config);
        }

        // seeks a folder for .conf files
        public static ConfigObject ApplyFromDirectory(string path, ConfigObject config = null, bool recursive = false) {
            if (!Directory.Exists(path)) {
                throw new System.Exception("no folder found in the given path");
            }

            if (config == null) {
                config = new ConfigObject();
            }

            var info = new DirectoryInfo(path);
            if (recursive) {
                foreach (DirectoryInfo dir in info.GetDirectories()) {
                    Console.WriteLine("reading in folder {0}", dir);
                    config = ApplyFromDirectoryInfo(dir, config, recursive);
                }
            }

            // find all files
            FileInfo[] files = info.GetFiles("*.conf",SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files) { 
                config = ApplyJsonFromFileInfo(file, config);
            }
            return config;
        }

        public static ConfigObject ApplyFromDirectoryInfo(DirectoryInfo info, ConfigObject config = null,
                bool recursive = false) {
            return ApplyFromDirectory(info.FullName, config, recursive);
        }

        public static ConfigObject ParseJson(string json) {
            string[] lines = json.Split(new[] {'\n'});
            // remove lines that start with a dash # character 
            IEnumerable<string> filtered = from l in lines
                where !(Regex.IsMatch(l, @"^\s*#(.*)"))
                select l;

            string filtered_json = string.Join("\n", filtered);


            dynamic parsed = JsonConvert.DeserializeObject<ExpandoObject>(filtered_json, new ExpandoObjectConverter());
            // convert the ExpandoObject to ConfigObject before returning
            return ConfigObject.FromExpando(parsed);
        }

        // overrides any default config specified in default.conf
        public static void SetDefaultConfig(dynamic config) {
            Default = config;
        }
    }
}