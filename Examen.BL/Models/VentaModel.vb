Imports System.ComponentModel.DataAnnotations

Public Class VentaModel
    Public Property ID As Integer

    <Display(Name:="Cliente")>
    Public Property IDCliente As Integer

    Public Property Fecha As DateTime = DateTime.Now
    Public Property Total As Decimal
    Public Property Items As VentaItemModel

End Class
