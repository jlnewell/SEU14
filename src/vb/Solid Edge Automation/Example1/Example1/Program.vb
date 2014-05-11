Imports SolidEdgeContrib
Imports SolidEdgeContrib.Extensions
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace Example1
	Friend Class Program
		<STAThread> _
		Shared Sub Main(ByVal args() As String) ' <-- In a console application, you must add the STAThread attribute!

			' Note that we're using SolidEdgeContrib.OleMessageFilter.
			OleMessageFilter.Register()

			' How long have you wished you could do this! Say thank you to SolidEdgeContrib.
            Dim application As SolidEdgeFramework.Application

            application = New SolidEdgeFramework.Application()

			' Get a reference to the Documents collection.
            Dim documents = application.Documents

			' Show Solid Edge.
			application.Visible = True

			' Demonstrate SolidEdgeContrib provided method extensions.
			DemoApplicationExtensionMethods(application, documents)

			' Turn off Solid Edge prompts.
			application.DisplayAlerts = False

			' Close all documents without saving.
			documents.Close()

			' Terminate Solid Edge.
			application.Quit()

			' Note that we're using SolidEdgeContrib.OleMessageFilter.
			OleMessageFilter.Unregister()
		End Sub

		Private Shared Sub DemoApplicationExtensionMethods(ByVal application As SolidEdgeFramework.Application, ByVal documents As SolidEdgeFramework.Documents)
			' Note the extension methods are only available when you use:
			' using SolidEdgeContrib.Extensions;

			' Add an assembly document.
			Dim assemblyDocument = documents.AddAssemblyDocument()

			' Always good to call DoIdle() after creating a new document.
			application.DoIdle()

			' Get a SolidEdgeFramework.SolidEdgeDocument typed reference to the active document.
			Dim activeDocument = application.GetActiveDocument()

			' Demonstrate generic GetActiveDocument extension method.
			Dim activeAssemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)()

			' Always good to call DoIdle() after creating a new document.
			application.DoIdle()

			' Add a draft document.
			Dim draftDocument = documents.AddDraftDocument()

			' Always good to call DoIdle() after creating a new document.
			application.DoIdle()

			' Demonstrate generic GetActiveDocument extension method.
			Dim activeDraftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)()

			' Add a part document.
			Dim partDocument = documents.AddPartDocument()

			' Always good to call DoIdle() after creating a new document.
			application.DoIdle()

			' Demonstrate generic GetActiveDocument extension method.
			Dim activePartDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)()

			' Get a reference to the RefPlanes collection.
			Dim refPlanes = activePartDocument.RefPlanes

			' Demonstrate using extension methods to easily get specific RefPlanes.
			Dim frontPlane = refPlanes.GetFrontPlane()
			Dim rightPlane = refPlanes.GetRightPlane()
			Dim topPlane = refPlanes.GetTopPlane()

			' Add a sheet metal document.
			Dim sheetMetalDocument = documents.AddSheetMetalDocument()

			' Always good to call DoIdle() after creating a new document.
			application.DoIdle()

			' Demonstrate generic GetActiveDocument extension method.
			Dim activeSheetMetalDocument = application.GetActiveDocument(Of SolidEdgePart.SheetMetalDocument)()

			' Demonstrate StartCommand extension methods.
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewBackView)
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewBottomView)
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewClipping)
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewDimetricView)
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewFrontView)
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView)
			application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewLeftView)

			' Get a strongly typed SolidEdgeFramework.ISEApplicationEvents_Event.
			Dim applicationEvents = application.GetApplicationEvents()

			' Get a strongly typed reference to the active environment.
			Dim environment = application.GetActiveEnvironment()

			' Get the seApplicationGlobalSystemInfo global parameter. Return type is an object.
			Dim globalSystemInfoObject = application.GetGlobalParameter(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo)

			' Get the seApplicationGlobalSystemInfo global parameter. Using the generic overload, return type is a string.
			Dim globalSystemInfoString = application.GetGlobalParameter(Of String)(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo)

			' Get an instance of a System.Diagnostics.Process that represents the Edge.exe process.
			Dim process = application.GetProcess()

			' Get an instance of a System.Version that represents the version of Solid Edge rather than just a string.
			Dim version = application.GetVersion()

		End Sub
	End Class
End Namespace
