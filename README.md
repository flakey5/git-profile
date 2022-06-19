# git-profile
Tool for managing multiple Git configurations. This only handles stuff in the `.gitconfig` file, not the authentication side of it.

## Usage
 * `git-profile help` - Print usage
 * `git-profile show` - Print what configuration you're currently using. Same as running the program without any arguments.
 * `git-profile list` - List all profiles
 * `git-profile switch <profile name>` - Switch to a profile
 * `git-profile create <profile name>` - Create a new profile
 * `git-profile edit <profile name>` - Edit a profile
 * `git-profile delete <profile name>` - Delete a profile

## Profile schema
Each profile has its own .json file. Here's an example of how it should look like:
```json
{
    "user": {
        "email": "testing@example.com",
        "name": "testing123"
    },
    "init": {
        "defaultBranch": "main"
    }
}
```

When this profile is generated, this will be written to your `.gitconfig`:

```
[user]
    email = testing@example.com
    name = testing123
[init]
    defaultBranch = main
```

## Compiling
This is just a .NET 6 console app, so open the solution in `src/` and build in Visual Studio or via the .NET CLI. The only NuGet dependency is Newtonsoft.Json.

## License
See [LICENSE](./LICENSE).