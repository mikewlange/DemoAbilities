# 1. Create a new ability 
1. Create a new Azure Function project
2. Add the `Livetiles.BotAbilityAzureFunctions` package via **Project > Manage NuGet Packages...**
3. Add two new class files (see below):
  - Startup.cs
  - Functions.cs
4. Add ability and action classes as needed (e.g. "HelloWorldDemoAbility.cs" and "Speak.cs" in the provided example)
5. Copy the code over from the Hello World project, changing the references so they match your ability and action names

### Notes
- A bug ([GitHub 3386](https://github.com/Azure/azure-functions-host/issues/3386)) in VS publish task for AF fails to publish the extensions.json file to Azure, and causes the functions to fail on startup. The following _Directory.Build.targets_ is required somewhere in project path or higher. This solution already contains the file:

```xml
<Project>
  <PropertyGroup>
    <_IsFunctionsSdkBuild Condition="$(_FunctionsTaskFramework) != ''">true</_IsFunctionsSdkBuild>
    <_FunctionsExtensionsDir>$(TargetDir)</_FunctionsExtensionsDir>
    <_FunctionsExtensionsDir Condition="$(_IsFunctionsSdkBuild) == 'true'">$(_FunctionsExtensionsDir)bin</_FunctionsExtensionsDir>
  </PropertyGroup>

  <Target Name="CopyExtensionsJson" AfterTargets="_GenerateFunctionsAndCopyContentFiles">
    <Message Importance="High" Text="Overwritting extensions.json file with one from build." />

    <Copy Condition="$(_IsFunctionsSdkBuild) == 'true' AND Exists('$(_FunctionsExtensionsDir)\extensions.json')"
          SourceFiles="$(_FunctionsExtensionsDir)\extensions.json"
          DestinationFiles="$(PublishDir)bin\extensions.json"
          OverwriteReadOnlyFiles="true"
          ContinueOnError="true"/>
  </Target>
</Project>
```

# 2. Create an AAD registration
1. Create a new AAD app registration in Azure (Azure Active Directory -> App Registrations -> New application registration)
2. Select the newly-created registration from the list and generate a client secret under "Certificates & secrets". Be sure to copy the secret to a safe location as it will no longer be available when this window is closed
3. Populate your project's /Configuration/settings.json file with the required fields (AAD directory ID, AAD client ID, AAD client secret generated above, messaging service client ID/base URL, and bot services client ID/base URL)

# 3. Populate deployment schema
1. Add the necessary fields to `ability-name`.json in your project's /Deploy folder; an example is below
```json
{
	"name": "Example Ability",
	"description": "An example ability.",
	"version": "0.0.1",
	"endpoint": "{{endpoint}}",
	"appId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
	"isPublic": true,
	"targets": [
		"something"
	],
	"allowMultiple": false,
  	"settings": [
		{
        		"key": "exampleSetting",
        		"type": 0,
        		"name": "Example Setting",
        		"description": "An example of a setting",
        		"required": true,
        		"readOnly": false,
        		"defaultValue": "Default"
      		}
	],
	"actions": [
    		{
      			"key": "DoSomething",
      			"name": "Do Something",
      			"description": "An example action.",
      			"keywords": [
        			"Do"
      			],
      			"sentenceStructures": [
        			"{Keyword} {Target}"
      			],
      			"parameters": [
        			{
          				"key": "exampleParameter",
          				"type": 0,
          				"name": "Example Parameter",
          				"description": "An example of a parameter",
          				"required": true,
          				"label": "Example parameter",
          				"prompt": "What is the value you would like to enter?",
          				"promptUser": true,
          				"examples": [],
          				"includeInLanguageModel": true
        			}
      			]
    		}
  	]
} 
```

# 4. Deploy project to Azure
1. Change `appName` in your Azure Resource Manager template
2. Right-click the "Deploy" folder, select "Deploy" -> "New", then follow the prompts, selecting your Azure subscription and resource group, then click "Deploy"

# 5. Install the Ability Lab CLI tool
1. The latest version can be found at https://www.nuget.org/packages/LiveTiles.AbilityLab
2. Run "dotnet tool install --global LiveTiles.AbilityLab --version 1.0.0-build.4601" in a PowerShell instance (increase the version number if necessary)
3. Configure with "lt-lab config add" and follow the prompts, inputting your desired designer name, along with the API URL, authority URL, client ID and API scope

# 6. Register the new ability
1. Run `lt-lab install <insert filepath of ability JSON spec here> --DesignerName <insert designer name (configured in previous step) here>`
2. Follow the link when prompted to login and enter the authorisation code
3. Copy the resulting ability ID into your <ability name>.cs class ("AbilityKey")

# 7. Publish the project
1. Right click the project in Visual Studio's solution explorer, then click Publish and follow the prompts

# Useful Methods

### Receiving Messages
Access variables supplied through bot designer prompts with `parameters.GetEntityValue<TYPE>(PARAMETER_KEY)`

### Posting Messages
Messages can be posted to the user using the `PostMessage` method and supplying the necessary parameters as follows:

```cs
PostMessage(ConversationContext context, string message, IEnumerable<MessageAttachment> attachment, CancellationToken token);
```

Note that `attachment` may be left as `null` if no attachments beyond the text message are necessary

# Troubleshooting & Debugging
### ngrok
ngrok allows you to test a local server via an externally accessible URL, giving you the ability to easily view requests and responses in order to debug more efficiently.

To install ngrok:
1. Visit https://ngrok.com/download and download the tool
2. Run your Azure Function App locally via Visual Studio
3. Run `./ngrok http 7071` (or whichever port the process is using)
4. Copy the ngrok-supplied forwarding URL (http://xxxxxxxx.ngrok.io) and append `/api/abilities`
5. Replace the registered ability endpoint in the DB with the new URL
6. Navigate to the web interface URL supplied by ngrok and begin testing
7. After testing and publishing to your Azure subscription, remember to switch the ability endpoint back to the original one

### 500 Errors
If you're seeing 500 errors, double-check that settings.json exists in the bin directory (under either Debug or Release, depending on your Azure publish settings, then under netcoreapp2.1/Configuration)

### 200 Errors
Ensure host.json in your project's root directory includes the appropriate HTTP route prefix, for example:
```json
{
  "version": "2.0",
  "extensions": {
    "http": {
      "routePrefix": "api/abilities"
    }
  }
}
```
