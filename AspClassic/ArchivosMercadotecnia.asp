<%
	Set myDiagramFileObject=Server.CreateObject("Scripting.FileSystemObject")
	Set myDiagramFolder=myDiagramFileObject.GetFolder(server.mappath("Files"))
	For each filefound in myDiagramFolder.files
		Response.Write "<li>"
		realFileName=filefound.Name
		realFileName=Replace(realFileName,"á","&aacute;")
		realFileName=Replace(realFileName,"ó","&oacute;")
		realFileName=Replace(realFileName,"í","&iacute;")
		realFileName=Replace(realFileName,"ú","&uacute;")
		realFileName=Replace(realFileName,"é","&eacute;")
		If myEditable="yes" then
			Response.Write "<a class='eliminarFile' fileName='"+realFileName+"'subject='"+subject+"'href='javascript:void(0);'></a>"
		end if
		Response.Write "<a href='Files/"+subject+"/" 
		Response.Write realFileName & "'>"
		Response.Write realFileName & "</a></li>"
	Next
%>