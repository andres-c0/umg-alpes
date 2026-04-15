Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class Control_CalidadServicio

        Private ReadOnly _datos As Control_CalidadDatos

        Public Sub New()
            _datos = New Control_CalidadDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Control_Calidad) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Control_Calidad)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Control_Calidad
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Control_Calidad)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Control_Calidad)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace