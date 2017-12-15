# uipath-activity
Windows Workflow activity to post items to Ingenia for UiPath

- Install Visual Studio Git extension + configure + clone repo (Default place for repos should be `C:\Users\USER\source\repos`)
- In VS, selecting `Build/Build Solution` should download and install the relevant package dependencies automatically (e.g. Newtonsoft.Json).
- The compiled DLLs will then be in `C:\...\IngeniaActivity\IngeniaActivity\bin\Debug`.
- All of these should be added to NuGet when building the Activity for UiPath Studio, as shown [here](https://www.uipath.com/kb-articles/how-to-execute-your-custom-code-in-a-workflow)
