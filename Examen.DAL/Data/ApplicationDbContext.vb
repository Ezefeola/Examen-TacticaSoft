Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net
Imports Examen.BL

Public Class ApplicationDbContext

    Public Shared Function ConnectionString() As String

        Return ConfigurationManager.ConnectionStrings("pruebademo").ConnectionString

    End Function

End Class
