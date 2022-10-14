Imports System, System.Data, System.IO.File, DNM.engine
Partial Class FCKeditor_editor_dnmBrowser_fileBrowser
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim ot As Table = browserTable
        If Not Session("FCKeditor:UserFilesPath") Is Nothing Then
            If Not IO.Directory.Exists(Server.MapPath(Session("FCKeditor:UserFilesPath"))) Then IO.Directory.CreateDirectory(Server.MapPath(Session("FCKeditor:UserFilesPath")))
            Dim nc As New TableCell, nr As New TableRow
            nc.Text = "<scr" & "ipt type=""text/javascript"">" & vbCrLf & "alert('mital');" & vbCrLf & "</scr" & "ipt>"
            nr.Controls.Add(nc)
            ot.Controls.Add(nr)
        End If
    End Sub
End Class
