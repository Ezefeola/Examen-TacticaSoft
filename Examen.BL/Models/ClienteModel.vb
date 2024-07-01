Imports System.ComponentModel.DataAnnotations

Public Class ClienteModel
    Public Property ID As Integer

    <Required(ErrorMessage:="El campo Cliente es obligatorio")>
    <StringLength(100, ErrorMessage:="El campo Cliente no puede tener más de 100 caracteres")>
    Public Property Cliente As String
    Public Property Telefono As String

    <StringLength(100, ErrorMessage:="El campo Correo no puede tener más de 100 caracteres")>
    Public Property Correo As String
End Class
