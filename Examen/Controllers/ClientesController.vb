Imports System.Web.Mvc
Imports Examen.BL
Imports Examen.DAL

Imports Newtonsoft.Json

Namespace Controllers
    Public Class ClientesController
        Inherits Controller

        Private ReadOnly _clienteRepository As IClienteRepository
        Public Sub New(clienteRepository As IClienteRepository)
            _clienteRepository = clienteRepository
        End Sub

        ' GET: Client
        Function Index(searchTerm As String, filterOption As String) As ActionResult
            Dim clientes As List(Of ClienteModel)

            ' Filtrar clientes según el término de búsqueda y opción de filtro
            If String.IsNullOrEmpty(searchTerm) Then
                clientes = _clienteRepository.GetAll()
            Else
                Select Case filterOption
                    Case "Cliente"
                        clientes = _clienteRepository.SearchByCliente(searchTerm)
                    Case "Telefono"
                        clientes = _clienteRepository.SearchByTelefono(searchTerm)
                    Case "Correo"
                        clientes = _clienteRepository.SearchByCorreo(searchTerm)
                    Case Else
                        clientes = _clienteRepository.GetAll()
                End Select
            End If

            ' Opciones para el dropdown de selección
            ViewBag.FilterOptions = New SelectList({"Cliente", "Telefono", "Correo"})

            Return View(clientes)
        End Function

        Function Search(searchTerm As String, filterOption As String) As JsonResult
            Dim clientes As List(Of ClienteModel)
            If String.IsNullOrEmpty(searchTerm) Then
                clientes = _clienteRepository.GetAll()
            Else
                'clientes = _clienteRepository.SearchClientes(searchTerm)
                Select Case filterOption
                    Case "Cliente"
                        clientes = _clienteRepository.SearchByCliente(searchTerm)
                    Case "Telefono"
                        clientes = _clienteRepository.SearchByTelefono(searchTerm)
                    Case "Correo"
                        clientes = _clienteRepository.SearchByCorreo(searchTerm)
                    Case Else
                        clientes = _clienteRepository.GetAll()
                End Select
            End If
            Return Json(clientes, JsonRequestBehavior.AllowGet)
        End Function

        Function Create() As ActionResult
            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Create(cliente As String, telefono As String, correo As String) As ActionResult

            If ModelState.IsValid Then

                Dim nuevoCliente As New ClienteModel With {
                    .Cliente = cliente,
                    .Telefono = telefono,
                    .Correo = correo
                }

                _clienteRepository.Add(nuevoCliente)

                Return RedirectToAction("Index")
            End If

            Return View("Create")
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Delete(ID As Integer) As ActionResult

            Try

                _clienteRepository.Delete(ID)
                Return RedirectToAction("Index")
            Catch ex As Exception

            End Try


            Return View("Index")

        End Function

        Function Update(ID As Integer) As ActionResult
            Dim cliente As ClienteModel = _clienteRepository.GetClienteById(ID)

            If cliente Is Nothing Then
                Return HttpNotFound()
            End If
            Return View(cliente)
        End Function


        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Update(id As Integer, cliente As String, telefono As String, correo As String) As ActionResult

            If ModelState.IsValid Then

                Dim ClienteActualizado As New ClienteModel With {
                    .ID = id,
                    .Cliente = cliente,
                    .Telefono = telefono,
                    .Correo = correo
                }

                _clienteRepository.Update(ClienteActualizado)

                Return RedirectToAction("Index")
            End If

            Return View("Update")
        End Function

    End Class

End Namespace