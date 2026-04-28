Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Inventario

Namespace Servicios
    Public Class Precio_HistoricoServicio

        Private ReadOnly _datos As Precio_HistoricoDatos

        Public Sub New()
            _datos = New Precio_HistoricoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Precio_Historico) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Precio_Historico)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Precio_Historico
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Precio_Historico)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Precio_Historico)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace