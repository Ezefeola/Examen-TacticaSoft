Imports Examen.BL
Imports System.Data.SqlClient

Public Class VentaRepository
    Implements IVentaRepository

    Private ReadOnly connectionString As String = ApplicationDbContext.ConnectionString()
    Public Function GetAll() As List(Of VentaModel) Implements IVentaRepository.GetAll
        Dim ventas As New List(Of VentaModel)()

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Dim query As String = "SELECT ID, IDCliente, Fecha, Total FROM ventas"

            Using cmd As New SqlCommand(query, conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim venta As New VentaModel()
                        venta.ID = Convert.ToInt32(reader("ID"))
                        venta.IDCliente = Convert.ToString(reader("IDCliente"))
                        venta.Fecha = reader("Fecha")
                        venta.Total = Convert.ToString(reader("Total"))

                        ventas.Add(venta)
                    End While
                End Using
            End Using
        End Using

        Return ventas
    End Function

    Public Function GetVentaById(ID As Integer) As VentaModel Implements IVentaRepository.GetVentaById
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT ID, IDCliente, Fecha, Total FROM ventas WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ID", ID)

                Dim ventaItem = GetVentaItemById(ID)

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim venta As New VentaModel With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .IDCliente = reader("IDCliente"),
                            .Fecha = reader("Fecha"),
                            .Total = reader("Total")
                        }
                        venta.Items = ventaItem

                        Return venta
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Using

    End Function

    Public Sub Add(venta As VentaModel) Implements IVentaRepository.Add
        If venta Is Nothing Then
            Throw New ArgumentNullException("venta", "El objeto venta no puede ser nulo")
        End If

        If venta.Items Is Nothing Then
            Throw New ArgumentException("La lista de ítems de venta no puede ser nula")
        End If

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim transaction As SqlTransaction = connection.BeginTransaction()

            Try
                Dim ventaId As Integer = CrearVenta(venta, connection, transaction)

                venta.Items.IDVenta = ventaId
                AddVentaItem(venta.Items, connection, transaction)

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error al agregar la venta", ex)
            End Try
        End Using
    End Sub

    Public Function CrearVenta(venta As VentaModel, connection As SqlConnection, transaction As SqlTransaction) As Integer Implements IVentaRepository.CrearVenta
        Dim query As String = "INSERT INTO ventas (IDCliente, Fecha, Total) VALUES (@IDCliente, @Fecha, @Total); SELECT CAST(SCOPE_IDENTITY() AS INT);"
        Using command As New SqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@IDCliente", venta.IDCliente)
            command.Parameters.AddWithValue("@Fecha", DateTime.Now)
            command.Parameters.AddWithValue("@Total", venta.Total)

            Return Convert.ToInt32(command.ExecuteScalar())
        End Using
    End Function

    Public Sub AddVentaItem(item As VentaItemModel, connection As SqlConnection, transaction As SqlTransaction) Implements IVentaRepository.AddVentaItem
        Dim query As String = "INSERT INTO ventasitems (IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal) " &
                                                                                                                                            "VALUES (@IDVenta, @IDProducto, @PrecioUnitario, @Cantidad, @PrecioTotal)"
        Using command As New SqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@IDVenta", item.IDVenta)
            command.Parameters.AddWithValue("@IDProducto", item.IDProducto)
            command.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario)
            command.Parameters.AddWithValue("@Cantidad", item.Cantidad)
            command.Parameters.AddWithValue("@PrecioTotal", item.PrecioTotal)

            command.ExecuteNonQuery()
        End Using
    End Sub

    Public Function GetVentaItemById(id As Integer) As VentaItemModel Implements IVentaRepository.GetVentaItemById
        Dim ventaItem As VentaItemModel = Nothing

        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT ID, IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal FROM ventasitems WHERE IDVenta = @ID"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ID", id)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        ventaItem = New VentaItemModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .IDVenta = Convert.ToInt32(reader("IDVenta")),
                            .IDProducto = Convert.ToInt32(reader("IDProducto")),
                            .PrecioUnitario = Convert.ToDecimal(reader("PrecioUnitario")),
                            .Cantidad = Convert.ToInt32(reader("Cantidad")),
                            .PrecioTotal = Convert.ToDecimal(reader("PrecioTotal"))
                        }
                    End If
                End Using
            End Using
        End Using

        Return ventaItem
    End Function
    Public Sub Delete(ID As Integer) Implements IVentaRepository.Delete
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim transaction As SqlTransaction = connection.BeginTransaction()

            Try
                ' Primero eliminar los ítems de venta
                Dim deleteItemsQuery As String = "DELETE FROM ventasitems WHERE IDVenta=@IDVenta"
                Using deleteItemsCommand As New SqlCommand(deleteItemsQuery, connection, transaction)
                    deleteItemsCommand.Parameters.AddWithValue("@IDVenta", ID)
                    deleteItemsCommand.ExecuteNonQuery()
                End Using

                ' Luego eliminar la venta
                Dim deleteVentaQuery As String = "DELETE FROM ventas WHERE ID=@ID"
                Using deleteVentaCommand As New SqlCommand(deleteVentaQuery, connection, transaction)
                    deleteVentaCommand.Parameters.AddWithValue("@ID", ID)
                    deleteVentaCommand.ExecuteNonQuery()
                End Using

                ' Confirmar la transacción
                transaction.Commit()
            Catch ex As Exception
                ' Revertir la transacción en caso de error
                transaction.Rollback()
                Throw New Exception("Error al eliminar la venta y sus ítems", ex)
            End Try
        End Using

    End Sub



    Public Sub Update(venta As VentaModel) Implements IVentaRepository.Update
        If venta Is Nothing Then
            Throw New ArgumentNullException("venta", "El objeto venta no puede ser nulo")
        End If

        If venta.Items Is Nothing Then
            Throw New ArgumentException("La lista de ítems de venta no puede ser nula")
        End If

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim transaction As SqlTransaction = connection.BeginTransaction()

            Try
                ActualizarVenta(venta, connection, transaction)
                ActualizarVentaItem(venta.Items, connection, transaction)



                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error al actualizar la venta", ex)
            End Try
        End Using
    End Sub

    Public Sub ActualizarVenta(venta As VentaModel, connection As SqlConnection, transaction As SqlTransaction) Implements IVentaRepository.ActualizarVenta
        Dim query As String = "UPDATE ventas SET Total = @Total WHERE ID = @ID"
        Using command As New SqlCommand(query, connection, transaction)
            'command.Parameters.AddWithValue("@IDCliente", venta.IDCliente)
            command.Parameters.AddWithValue("@Total", venta.Total)
            command.Parameters.AddWithValue("@ID", venta.ID)

            command.ExecuteNonQuery()
        End Using
    End Sub
    Public Sub ActualizarVentaItem(ventaItem As VentaItemModel, connection As SqlConnection, transaction As SqlTransaction) Implements IVentaRepository.ActualizarVentaItem
        Dim query As String = "UPDATE ventasitems SET IDProducto=@IDProducto, PrecioUnitario=@PrecioUnitario, Cantidad=@Cantidad, PrecioTotal=@PrecioTotal WHERE IDVenta = @ID"
        Using command As New SqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@IDProducto", ventaItem.IDProducto)
            command.Parameters.AddWithValue("@PrecioUnitario", ventaItem.PrecioUnitario)
            command.Parameters.AddWithValue("@Cantidad", ventaItem.Cantidad)
            command.Parameters.AddWithValue("@PrecioTotal", ventaItem.PrecioTotal)
            command.Parameters.AddWithValue("@ID", ventaItem.IDVenta)

            command.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub EliminarVentaItems(ventaId As Integer, connection As SqlConnection, transaction As SqlTransaction) Implements IVentaRepository.EliminarVentaItems
        Dim query As String = "DELETE FROM ventasitems WHERE IDVenta = @IDVenta"
        Using command As New SqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@IDVenta", ventaId)

            command.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub AddOrUpdateVentaItem(item As VentaItemModel, connection As SqlConnection, transaction As SqlTransaction) Implements IVentaRepository.AddOrUpdateVentaItem
        Dim query As String = "INSERT INTO ventasitems (IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal) " &
                                                                                                                                                  "VALUES (@IDVenta, @IDProducto, @PrecioUnitario, @Cantidad, @PrecioTotal)"
        Using command As New SqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@IDVenta", item.IDVenta)
            command.Parameters.AddWithValue("@IDProducto", item.IDProducto)
            command.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario)
            command.Parameters.AddWithValue("@Cantidad", item.Cantidad)
            command.Parameters.AddWithValue("@PrecioTotal", item.PrecioTotal)

            command.ExecuteNonQuery()
        End Using
    End Sub

    Public Function SearchByCliente(searchTerm As String) As List(Of VentaModel) Implements IVentaRepository.SearchByCliente
        Dim ventas As New List(Of VentaModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT ventas.ID, ventas.IDCliente, ventas.Fecha, ventas.Total " &
                              "FROM ventas " &
                              "INNER JOIN clientes ON ventas.IDCliente = clientes.ID " &
                              "WHERE clientes.Cliente LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ventas.Add(New VentaModel() With {
                        .ID = Convert.ToInt32(reader("ID")),
                        .IDCliente = Convert.ToInt32(reader("IDCliente")),
                        .Fecha = reader("Fecha"),
                        .Total = Convert.ToDecimal(reader("Total"))
                    })
                    End While
                End Using
            End Using
        End Using
        Return ventas
    End Function

    Public Function SearchByFecha(searchTerm As String) As List(Of VentaModel) Implements IVentaRepository.SearchByFecha
        Dim clientes As New List(Of VentaModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM ventas WHERE Fecha LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        clientes.Add(New VentaModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .IDCliente = reader("IDCliente"),
                            .Fecha = reader("Fecha"),
                            .Total = reader("Total")
                        })
                    End While
                End Using
            End Using
        End Using
        Return clientes
    End Function

End Class
