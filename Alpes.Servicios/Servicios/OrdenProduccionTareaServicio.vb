Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class OrdenProduccionTareaServicio

        Private ReadOnly _datos As OrdenProduccionTareaDatos

        Public Sub New()
            _datos = New OrdenProduccionTareaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As OrdenProduccionTarea) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As OrdenProduccionTarea) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal opTareaId As Integer) As Boolean
            Return _datos.Eliminar(opTareaId)
        End Function

        Public Function ObtenerPorId(ByVal opTareaId As Integer) As OrdenProduccionTarea
            Return _datos.ObtenerPorId(opTareaId)
        End Function

        Public Function Listar() As List(Of OrdenProduccionTarea)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of OrdenProduccionTarea)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace