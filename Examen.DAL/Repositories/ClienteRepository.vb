Imports Examen.BL
Imports System.Configuration
Imports System.Data.SqlClient

Public Class ClienteRepository
    Implements IClienteRepository

    Private ReadOnly connectionString As String = ApplicationDbContext.ConnectionString()

    Public Function GetAll() As List(Of ClienteModel) Implements IClienteRepository.GetAll
        Dim clientes As New List(Of ClienteModel)()


        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Dim query As String = "SELECT ID, Cliente, Telefono, Correo FROM clientes"

            Using cmd As New SqlCommand(query, conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim cliente As New ClienteModel()
                        cliente.ID = Convert.ToInt32(reader("ID"))
                        cliente.Cliente = Convert.ToString(reader("Cliente"))
                        cliente.Telefono = Convert.ToString(reader("Telefono"))
                        cliente.Correo = Convert.ToString(reader("Correo"))

                        clientes.Add(cliente)
                    End While
                End Using
            End Using
        End Using

        Return clientes
    End Function

    Public Function GetClienteById(ID As Integer) As ClienteModel Implements IClienteRepository.GetClienteById
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT ID, Cliente, Telefono, Correo FROM clientes WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ID", ID)

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim cliente As New ClienteModel With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Cliente = reader("Cliente"),
                            .Telefono = reader("Telefono"),
                            .Correo = reader("Correo")
                        }
                        Return cliente
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Using

    End Function

    Public Sub Add(cliente As ClienteModel) Implements IClienteRepository.Add
        If cliente Is Nothing Then
            Throw New ArgumentNullException("cliente", "El objeto cliente no puede ser nulo")
        End If

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "INSERT INTO clientes (Cliente, Telefono, Correo) VALUES (@Cliente, @Telefono, @Correo)"
            Using command As New SqlCommand(query, connection)

                command.Parameters.AddWithValue("@Cliente", cliente.Cliente)
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono)
                command.Parameters.AddWithValue("@Correo", cliente.Correo)

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Delete(ID As Integer) Implements IClienteRepository.Delete
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "DELETE FROM clientes WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)

                command.Parameters.AddWithValue("@ID", ID)

                command.ExecuteNonQuery()
            End Using
        End Using

    End Sub

    Public Sub Update(cliente As ClienteModel) Implements IClienteRepository.Update
        If cliente Is Nothing Then
            Throw New ArgumentNullException("cliente", "El objeto cliente no puede ser nulo")
        End If


        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "UPDATE clientes SET Cliente=@Cliente, Telefono=@Telefono, Correo=@Correo WHERE ID=@ID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Cliente", cliente.Cliente)
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono)
                command.Parameters.AddWithValue("@Correo", cliente.Correo)
                command.Parameters.AddWithValue("@ID", cliente.ID)

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function SearchByCliente(searchTerm As String) As List(Of ClienteModel) Implements IClienteRepository.SearchByCliente
        Dim clientes As New List(Of ClienteModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM Clientes WHERE Cliente LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        clientes.Add(New ClienteModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Cliente = reader("Cliente"),
                            .Telefono = reader("Telefono"),
                            .Correo = reader("Correo")
                        })
                    End While
                End Using
            End Using
        End Using
        Return clientes
    End Function

    Public Function SearchByTelefono(searchTerm As String) As List(Of ClienteModel) Implements IClienteRepository.SearchByTelefono
        Dim clientes As New List(Of ClienteModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM Clientes WHERE Telefono LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        clientes.Add(New ClienteModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Cliente = reader("Cliente"),
                            .Telefono = reader("Telefono"),
                            .Correo = reader("Correo")
                        })
                    End While
                End Using
            End Using
        End Using
        Return clientes
    End Function

    Public Function SearchByCorreo(searchTerm As String) As List(Of ClienteModel) Implements IClienteRepository.SearchByCorreo
        Dim clientes As New List(Of ClienteModel)
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM Clientes WHERE Correo LIKE @SearchTerm"
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        clientes.Add(New ClienteModel() With {
                            .ID = Convert.ToInt32(reader("ID")),
                            .Cliente = reader("Cliente"),
                            .Telefono = reader("Telefono"),
                            .Correo = reader("Correo")
                        })
                    End While
                End Using
            End Using
        End Using
        Return clientes
    End Function
End Class
