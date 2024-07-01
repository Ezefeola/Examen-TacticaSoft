Imports Examen.BL

Public Interface IClienteRepository
    Sub Add(cliente As ClienteModel)
    Sub Delete(ID As Integer)
    Sub Update(cliente As ClienteModel)
    Function GetAll() As List(Of ClienteModel)
    Function GetClienteById(ID As Integer) As ClienteModel
    Function SearchByCliente(searchTerm As String) As List(Of ClienteModel)
    Function SearchByTelefono(searchTerm As String) As List(Of ClienteModel)
    Function SearchByCorreo(searchTerm As String) As List(Of ClienteModel)
End Interface
