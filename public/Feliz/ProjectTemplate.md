# Project Template [![Nuget](https://img.shields.io/nuget/v/Feliz.Template.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.Template)

Feliz includes a template that scaffolds a full-fledged frontend application in one go. This is the easiest way to get started with Feliz and Elmish. The template also includes a test project that can run live in the browser or using node.js and mocha.

### Install the template

Install the template into your dotnet project templates as follows
```
dotnet new -i Feliz.Template
```
Now you should be able to create a new Feliz project as follows:
```
dotnet new feliz -n AwesomeApp
```
Now you are good to go. This will scaffold the application inside the newly created `AwesomeApp` directory.

### Update the template

Remember to update the template every once in a while especially before you scaffold a new project so that you get the latest updates from the template. Updating the template is a matter of uninstalling then re-installing it:
```
dotnet new -u Feliz.Template
dotnet new -i Feliz.Template
```