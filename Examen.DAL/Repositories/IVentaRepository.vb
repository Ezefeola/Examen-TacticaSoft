Imports System.Data.SqlClient
Imports Examen.BL

Public Interface IVentaRepository
    Sub ActualizarVenta(venta As VentaModel, connection As SqlConnection, transaction As SqlTransaction)
    Sub ActualizarVentaItem(ventaItem As VentaItemModel, connection As SqlConnection, transaction As SqlTransaction)
    Sub Add(venta As VentaModel)
    Sub AddOrUpdateVentaItem(item As VentaItemModel, connection As SqlConnection, transaction As SqlTransaction)
    Sub AddVentaItem(item As VentaItemModel, connection As SqlConnection, transaction As SqlTransaction)
    Sub Delete(ID As Integer)
    Sub EliminarVentaItems(ventaId As Integer, connection As SqlConnection, transaction As SqlTransaction)
    Sub Update(venta As VentaModel)
    Function CrearVenta(venta As VentaModel, connection As SqlConnection, transaction As SqlTransaction) As Integer
    Function GetAll() As List(Of VentaModel)
    Function GetVentaById(ID As Integer) As VentaModel
    Function GetVentaItemById(id As Integer) As VentaItemModel
    Function SearchByCliente(searchTerm As String) As List(Of VentaModel)
    Function SearchByFecha(searchTerm As String) As List(Of VentaModel)
End Interface
