Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class Recepcion_MaterialServicio

        Private ReadOnly _datos As Recepcion_MaterialDatos

        Public Sub New()
            _datos = New Recepcion_MaterialDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Recepcion_Material) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Recepcion_Material)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Recepcion_Material
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Recepcion_Material)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Recepcion_Material)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace