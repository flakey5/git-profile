# git-profile
Tool for managing multiple Git configurations. This only handles stuff in the `~/.gitconfig` file and not the authentication side of it.

## Usage
 * `git-profile help` - Print usage
 * `git-profile show` - Print what configuration you're currently using. Same as running the program without any arguments.
 * `git-profile list` - List all profiles
 * `git-profile switch <profile name>` - Switch to a profile
 * `git-profile create <profile name>` - Create a new profile. This does not switch you to the new profile.
 * `git-profile edit <profile name>` - Edit a profile
 * `git-profile delete <profile name>` - Delete a profile

## Getting started
When you first run it, it will ask you for your preferred text editor and what you want your first profile to be named. Then it's just a matter of filling out the configuration file which is just a `.gitconfig` file but named (ex/ `testing123.gitconfig`).
Once you're done with that just run `git-profile switch <your profile name>` and you're good to go.

## Compiling
This is just a .NET 6 console app, so open the solution in `src/` and build in Visual Studio or restore & build using the .NET CLI. The only NuGet dependency is Newtonsoft.Json.

## License
See [LICENSE](./LICENSE).