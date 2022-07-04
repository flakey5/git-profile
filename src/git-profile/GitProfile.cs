using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;

namespace gitprofile {
    internal class GitProfile {
        private static readonly string ASSEMBLY_DIRECTORY = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        private static readonly string PROFILES_DIRECTORY = Path.Join(ASSEMBLY_DIRECTORY, "profiles");
        private static readonly string CONFIG_FILE_PATH = Path.Join(ASSEMBLY_DIRECTORY, "config.json");

        private static Configuration configuration;

        public static void run(string[] args) {
            if (!File.Exists(CONFIG_FILE_PATH)) {
                initialSetup();
                return;
            }

            Configuration? configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(CONFIG_FILE_PATH));
            if (configuration == null) {
                Console.WriteLine("Failed parsing configuration file");
                return;
            }

            GitProfile.configuration = configuration;

            if (args.Length == 0) {
                printCurrentProfile();
            } else if (args.Length == 1) {
                switch (args[0].ToLower()) {
                    case "help":
                    case "--help":
                    case "-?":
                    case "--?":
                    case "/help":
                    case "/?":
                        printHelp();
                        break;
                    case "show":
                        printCurrentProfile();
                        break;
                    case "list":
                        printProfiles();
                        break;
                    default:
                        printHelp();
                        break;
                }
            } else {
                string profileName = args[1].ToLower();

                switch (args[0].ToLower()) {
                    case "switch":
                        switchProfile(profileName);
                        break;
                    case "create":
                        createProfile(profileName);
                        break;
                    case "edit":
                        editProfile(profileName);
                        break;
                    case "delete":
                        deleteProfile(profileName);
                        break;
                    default:
                        printHelp();
                        break;
                }
            }
        }

        private static void printHelp() {
            Console.WriteLine("git-profile - Tool for managing multiple Git configurations");
            Console.WriteLine("Usage: git-profile <action> [action arguments]");
            Console.WriteLine("  Actions:");
            Console.WriteLine("    help - Print this message");
            Console.WriteLine("    show - Print what configuration you're currently using. Same as running the program without any arguments.");
            Console.WriteLine("    list - List all profiles");
            Console.WriteLine("    switch <profile name> - Switch to a profile");
            Console.WriteLine("    create <profile name> - Create a new profile");
            Console.WriteLine("    edit <profile name> - Edit an existing profile");
            Console.WriteLine("    delete <profile name> - Delete a profile");
        }

        private static void initialSetup() {
            Console.WriteLine("Hello! It looks like this is your first time running git-profile, so please select your preferred text editor and create your first profile!");

            if (!Directory.Exists(PROFILES_DIRECTORY))
                Directory.CreateDirectory(PROFILES_DIRECTORY);

            string preferredTextEditor;
            do {
                Console.WriteLine("Please enter the absolute path of your preferred text editor (ex/ C:\\Windows\\notepad.exe)");
                preferredTextEditor = Console.ReadLine()!;
            } while (string.IsNullOrEmpty(preferredTextEditor));

            string firstProfileName;
            do {
                Console.WriteLine("Please enter the name of what you want your first profile to be called");
                firstProfileName = Console.ReadLine()!;
            } while (string.IsNullOrEmpty(firstProfileName));

            Configuration configuration = new Configuration() {
                preferredTextEditor = preferredTextEditor,
                currentProfile = firstProfileName
            };
            GitProfile.configuration = configuration;
            File.WriteAllText(CONFIG_FILE_PATH, JsonConvert.SerializeObject(configuration));

            createProfile(firstProfileName);
        }

        private static void printCurrentProfile() {
            Console.WriteLine($"Current profile: {configuration.currentProfile}");
        }

        private static void printProfiles() {
            Console.WriteLine("Available profiles:");

            foreach (string fileName in Directory.GetFiles(PROFILES_DIRECTORY, "*.gitconfig")) {
                Console.WriteLine(Path.GetFileNameWithoutExtension(fileName));
            }
        }

        private static void switchProfile(string name) {
            string profilePath = Path.Join(PROFILES_DIRECTORY, $"{name}.gitconfig");
            if (!File.Exists(profilePath)) {
                Console.WriteLine($"Profile with name {name} does not exist!");
                return;
            }

            string gitConfigPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gitconfig");
            if (!File.Exists(gitConfigPath)) {
                Console.WriteLine($"Failed finding .gitconfig ({gitConfigPath})");
                return;
            }

            File.Delete(gitConfigPath);
            File.Copy(profilePath, gitConfigPath);
            
            configuration.currentProfile = name;
            File.WriteAllText(CONFIG_FILE_PATH, JsonConvert.SerializeObject(configuration));

            Console.WriteLine($"Switched to profile {name}");
        }

        private static void createProfile(string name) {
            string profilePath = Path.Join(PROFILES_DIRECTORY, $"{name}.gitconfig");
            if (File.Exists(profilePath)) {
                Console.WriteLine($"Profile with name {name} already exists!");
                return;
            }

            File.Create(profilePath);
            Process.Start(configuration.preferredTextEditor, profilePath);
        }

        private static void editProfile(string name) {
            string profilePath = Path.Join(PROFILES_DIRECTORY, $"{name}.gitconfig");
            if (!File.Exists(profilePath)) {
                Console.WriteLine($"Profile with name {name} does not exist!");
                return;
            }

            Process.Start(configuration.preferredTextEditor, profilePath);
        }

        private static void deleteProfile(string name) {
            string profilePath = Path.Join(PROFILES_DIRECTORY, $"{name}.gitconfig");
            if (!File.Exists(profilePath)) {
                Console.WriteLine($"Profile with name {name} does not exist!");
                return;
            }

            File.Delete(profilePath);
            Console.WriteLine($"Successfully deleted profile {name}");
        }
    }
}