Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Metodo_PagoServicio

        Private ReadOnly _datos As Metodo_PagoDatos

        Public Sub New()
            _datos = New Metodo_PagoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Metodo_Pago) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Metodo_Pago)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Metodo_Pago
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Metodo_Pago)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Metodo_Pago)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace