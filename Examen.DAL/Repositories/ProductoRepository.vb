Imports Examen.BL
Imports System.Data.SqlClient

Public Class ProductoRepository
    Implements IProductoRepository

    Private ReadOnly connectionString As String = ApplicationDbContext.ConnectionString()

    Public Function GetAll() As List(Of ProductoModel) Implements IProductoRepository.GetAll
        Dim productos As New List(Of ProductoModel)()


        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Dim query As String = "SELECT ID, Nombre, Precio, Categoria FROM productos"

            Using cmd As New SqlCommand(query, conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim producto As New ProductoModel()
                        producto.ID = Convert.ToInt32(reader("ID"))
                        producto.Nombre = reader("Nombre")
                        producto.Precio = reader("Precio")
                        producto.Categoria = reader("Categoria")

                        productos.Add(producto)
                    End While
                End Using
            End Using
        End Using

        Return productos
    End Function

    Public Function GetProductoById(ID As Integer) As ProductoModel Implements IProductoRepository.GetProductoById
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT ID, Nombre, Precio, Categoria FROM productos WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ID", ID)

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim producto As New ProductoModel With {
.ID = Convert.ToInt32(reader("ID")),
                            .Nombre = reader("Nombre"),
                            .Precio = reader("Precio"),
                            .Categoria = reader("Categoria")
                        }
                        Return producto
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Using

    End Function

    Public Sub Add(producto As ProductoModel) Implements IProductoRepository.Add
        If producto Is Nothing Then
            Throw New ArgumentNullException("cliente", "El objeto cliente no puede ser nulo")
        End If

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "INSERT INTO productos (Nombre, Precio, Categoria) VALUES (@Nombre, @Precio, @Categoria)"
            Using command As New SqlCommand(query, connection)

                command.Parameters.AddWithValue("@Nombre", producto.Nombre)
                command.Parameters.AddWithValue("@Precio", producto.Precio)
                command.Parameters.AddWithValue("@Categoria", producto.Categoria)

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Delete(ID As Integer) Implements IProductoRepository.Delete
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "DELETE FROM productos WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)

                command.Parameters.AddWithValue("@ID", ID)

                command.ExecuteNonQuery()
            End Using
        End Using

    End Sub

    Public Sub Update(producto As ProductoModel) Implements IProductoRepository.Update
        If producto Is Nothing Then
            Throw New ArgumentNullException("cliente", "El objeto cliente no puede ser nulo")
        End If


        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "UPDATE productos SET Nombre=@Nombre, Precio=@Precio, Categoria=@Categoria WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Nombre", producto.Nombre)
                command.Parameters.AddWithValue("@Precio", producto.Precio)
                command.Parameters.AddWithValue("@Categoria", producto.Categoria)
                command.Parameters.AddWithValue("@ID", producto.ID)

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function SearchByCliente(searchTerm As String) As List(Of ProductoModel) Implements IProductoRepository.SearchByNombre
        Dim productos As New List(Of ProductoModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM productos WHERE Nombre LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        productos.Add(New ProductoModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Nombre = reader("Nombre"),
                            .Precio = reader("Precio"),
                            .Categoria = reader("Categoria")
                        })
                    End While
                End Using
            End Using
        End Using
        Return productos
    End Function

    Public Function SearchByPrecio(searchTerm As Decimal) As List(Of ProductoModel) Implements IProductoRepository.SearchByPrecio
        Dim productos As New List(Of ProductoModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM productos WHERE Precio LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        productos.Add(New ProductoModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Nombre = reader("Nombre"),
                            .Precio = reader("Precio"),
                            .Categoria = reader("Categoria")
                        })
                    End While
                End Using
            End Using
        End Using
        Return productos
    End Function

    Public Function SearchByPrecio(searchTerm As String) As List(Of ProductoModel) Implements IProductoRepository.SearchByCategoria
        Dim productos As New List(Of ProductoModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM productos WHERE Categoria LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        productos.Add(New ProductoModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Nombre = reader("Nombre"),
                            .Precio = reader("Precio"),
                            .Categoria = reader("Categoria")
                        })
                    End While
                End Using
            End Using
        End Using
        Return productos
    End Function

End Class
