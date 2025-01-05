using Deepslate.LauncherLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepslate.LauncherLib
{
    public class LaunchCommandBuilder
    {
        private string javaPath;
        private string mainClass;
        private StringBuilder jvmArgumentBuilder;
        private StringBuilder gameArgumentBuilder;

        public LaunchCommandBuilder(string javaPath, string mainClass) 
        {
            jvmArgumentBuilder = new StringBuilder();
            gameArgumentBuilder = new StringBuilder();
        }

        public LaunchCommandBuilder JavaNativesPath(string path)
        {
            return AppendJVMArgument("Djava.library.path", path);
        }

        public LaunchCommandBuilder LauncherBrand(string brand)
        {
            return AppendJVMArgument("Dminecraft.launcher.brand", brand);
        }

        public LaunchCommandBuilder LauncherVersion(string version)
        {
            return AppendJVMArgument("Djava.library.path", version);
        }

        /// <summary>
        /// Used to define the file path to the main class
        /// </summary>
        /// <param name="classPath"></param>
        /// <returns></returns>
        public LaunchCommandBuilder ClassPath(string classPath)
        {
            return AppendJVMArgument("-cp ", classPath, ' ');
        }

        public LaunchCommandBuilder OperatingSystem(OsType osType)
        {
            switch(osType)
            {
                case OsType.Windows:
                    return AppendJVMArgument("Dos.name", "Windows 10").AppendJVMArgument("Dos.version", "10.0");
                case OsType.OSX:
                    return AppendJVMArgument("XstartOnFirstThread");
                default:
                    return this;
            }
        }

        public LaunchCommandBuilder ForceGpuOptimus()
        {
            return AppendJVMArgument("XX:HeapDumpPath", "MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump");
        }

        public LaunchCommandBuilder StackSize(int size)
        {
            // TODO: properly format the size
            return AppendJVMArgument("Xss", size.ToString());
        }

        public LaunchCommandBuilder MaxHeapSize(int size)
        {
            return AppendJVMArgument("Xmx", size.ToString());
        }

        public LaunchCommandBuilder MinHeapSize(int size)
        {
            return AppendJVMArgument("Xms", size.ToString());
        }

        public LaunchCommandBuilder LauncherUuid(string? uuid = null)
        {
            var value = uuid ?? Guid.NewGuid().ToString();
            return AppendGameArgument("clientId", value);
        }

        public LaunchCommandBuilder SignerId(string id)
        {
            return AppendGameArgument("xuid", id);
        }

        public LaunchCommandBuilder Username(string username)
        {
            return AppendGameArgument("username", username);
        }

        public LaunchCommandBuilder VersionName(string version)
        {
            return AppendGameArgument("version", version);
        }

        public LaunchCommandBuilder GameDirectory(string directory)
        {
            return AppendGameArgument("gameDir", directory);
        }

        public LaunchCommandBuilder AssetsDirectory(string directory)
        {
            return AppendGameArgument("assetsDir", directory);
        }

        public LaunchCommandBuilder AssetsIndex(string index)
        {
            return AppendGameArgument("assetsIndex", index);
        }

        public LaunchCommandBuilder PlayerUuid(string uuid)
        {
            return AppendGameArgument("uuid", uuid);
        }

        public LaunchCommandBuilder AccessToken(string token)
        {
            return AppendGameArgument("accessToken", token);
        }

        public LaunchCommandBuilder AuthenticationType(UserType type)
        {
            switch(type)
            {
                case UserType.MicrosoftAuthentication:
                    return AppendGameArgument("userType", "mojang");
                case UserType.LegacyMinecraftAuthentication:
                    return AppendGameArgument("userType", "legacy");
                case UserType.MojangAuthentication:
                    return AppendGameArgument("userType", "microsoft");
                default:
                    return this;
            }
        }

        public LaunchCommandBuilder VersionType(string type)
        {
            return AppendGameArgument("versionType", type);
        }

        public LaunchCommandBuilder Demo()
        {
            return AppendGameArgument("demo");
        }

        public LaunchCommandBuilder Width(int width)
        {
            return AppendGameArgument("width", width.ToString());
        }

        public LaunchCommandBuilder Height(int height)
        {
            return AppendGameArgument("height", height.ToString());
        }

        public LaunchCommandBuilder DisableMultiplayer()
        {
            return AppendGameArgument("disableMultiplayer");
        }

        public LaunchCommandBuilder DisableChat()
        {
            return AppendGameArgument("disableChat");
        }

        public LaunchCommandBuilder QuickPlayPath(string path)
        {
            return AppendGameArgument("quickPlayPath", path);
        }

        public LaunchCommandBuilder QuickPlayWorld(string worldOrPath)
        {
            return AppendGameArgument("quickPlayWorld", worldOrPath);
        }

        public LaunchCommandBuilder QuickPlayServer(string server)
        {
            return AppendGameArgument("quickPlayMultiplayer", server);
        }

        public LaunchCommandBuilder QuickPlayRealms(string realm)
        {
            return AppendGameArgument("quickPlayPort", realm);
        }

        private LaunchCommandBuilder AppendJVMArgument(string argument, string? value = null, char separator = '=')
        {
            jvmArgumentBuilder.Append('-');
            jvmArgumentBuilder.Append(argument);
            if (value is not null)
            {
                jvmArgumentBuilder.Append(separator);
                jvmArgumentBuilder.Append(value);
            }
            jvmArgumentBuilder.Append(' ');
            return this;
        }

        private LaunchCommandBuilder AppendGameArgument(string argument, string? value = null, char separator = ' ')
        {
            gameArgumentBuilder.Append('-');
            gameArgumentBuilder.Append('-');
            gameArgumentBuilder.Append(argument);
            if (value is not null)
            {
                gameArgumentBuilder.Append(separator);
                gameArgumentBuilder.Append(value);
            }
            gameArgumentBuilder.Append(' ');
            return this;
        }

        public string Build()
        {
            return new StringBuilder()
                .Append(javaPath)
                .Append(' ')
                .Append(jvmArgumentBuilder.ToString())
                .Append(' ')
                .Append(mainClass)
                 .Append(' ')
                .Append(gameArgumentBuilder.ToString())
                .ToString();
        }

        public override string ToString() => Build();
    }
}
