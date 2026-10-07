Imports System.ComponentModel
Imports System.Globalization
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Reflection

Namespace Models
#Region "<Interface>"
    Public Interface IEntityCommand

    End Interface

    Public Interface IEntityCommandAsync

    End Interface

#Region "<CrudOperations>"
    Public Interface IEntityCommandRead(Of T As Entity) : Inherits IEntityCommand
        Sub Read(Entity As T)
    End Interface

    Public Interface IEntityCommandReadAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function ReadAsync(Entity As T) As Task
    End Interface

    Public Interface IEntityCommandReadMany(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommand
        Sub ReadMany(EntityCollection As T)
    End Interface

    Public Interface IEntityCommandReadManyAsync(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommandAsync
        Function ReadManyAsync(EntityCollection As T) As Task
    End Interface

    Public Interface IEntityCommandCreate(Of T As Entity) : Inherits IEntityCommand
        Sub Create(Entity As T)
    End Interface

    Public Interface IEntityCommandCreateAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function CreateAsync(Entity As T) As Task
    End Interface

    Public Interface IEntityCommandCreateMany(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommand
        Sub CreateMany(EntityCollection As T)
    End Interface

    Public Interface IEntityCommandCreateManyAsync(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommandAsync
        Function CreateManyAsync(EntityCollection As T) As Task
    End Interface

    Public Interface IEntityCommandUpdate(Of T As Entity) : Inherits IEntityCommand
        Sub Update(Entity As T)
    End Interface

    Public Interface IEntityCommandUpdateAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function UpdateAsync(Entity As T) As Task
    End Interface

    Public Interface IEntityCommandUpdateMany(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommand
        Sub UpdateMany(EntityCollection As T)
    End Interface

    Public Interface IEntityCommandUpdateManyAsync(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommandAsync
        Function UpdateManyAsync(EntityCollection As T) As Task
    End Interface

    Public Interface IEntityCommandDelete(Of T As Entity) : Inherits IEntityCommand
        Sub Delete(Entity As T)
    End Interface

    Public Interface IEntityCommandDeleteAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function DeleteAsync(Entity As T) As Task
    End Interface

    Public Interface IEntityCommandDeleteMany(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommand
        Sub DeleteMany(EntityCollection As T)
    End Interface

    Public Interface IEntityCommandDeleteManyAsync(Of T As IEnumerable(Of Entity)) : Inherits IEntityCommandAsync
        Function DeleteManyAsync(EntityCollection As T) As Task
    End Interface
#End Region

#Region "<SpecialisedOperations>"
    Public Interface IEntityCommandExist(Of T As Entity) : Inherits IEntityCommand
        Function Exist(Entity As T) As Boolean
    End Interface

    Public Interface IEntityCommandExistAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function ExistAsync(Entity As T) As Task(Of Boolean)
    End Interface

    Public Interface IEntityCommandAll(Of T As Entity) : Inherits IEntityCommand
        Function All() As IEnumerable(Of T)
    End Interface

    Public Interface IEntityCommandAllAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function AllAsync() As Task(Of IEnumerable(Of T))
    End Interface

    Public Interface IEntityCommandRelated(Of T As Entity) : Inherits IEntityCommand
        Function Related(Entity As T) As IEnumerable(Of T)
    End Interface

    Public Interface IEntityCommandRelatedAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function RelatedAsync(Entity As T) As Task(Of IEnumerable(Of T))
    End Interface

    Public Interface IEntityCommandUpsert(Of T As Entity) : Inherits IEntityCommand
        Sub Upsert(Entity As T)
    End Interface

    Public Interface IEntityCommandUpsertAsync(Of T As Entity) : Inherits IEntityCommandAsync
        Function UpsertAsync(Entity As T) As Task
    End Interface
#End Region

    Public Interface IEntityCommandFile : Inherits IEntityCommand
        Property FilePath As String
    End Interface

    Public Interface IEntityCommandLog : Inherits IEntityCommand
    End Interface

    Public Interface IEntityCommandSoap(Of T As Class) : Inherits IEntityCommand
        Property Client As T
        Property SecurityToken As String
    End Interface

    Public Interface IEntityCommandXml(Of T As Entity) : Inherits IEntityCommand
        Property Count As Integer
        Property LastId As Integer
        Property IdIncrement As Integer
        Function IndexOf(Entity As T) As Integer
        Sub ResetCount()
        Sub ResetLastId()
        Sub ResetLastId(LastId As Integer)
        'Sub TransformAndValidate(Handler As ValidationEventHandler)
    End Interface

    Public Interface IEntityCommandXmlDocument(Of T As Entity) : Inherits IEntityCommand
        Property Count As Integer
        Property LastId As Integer
        Property IdIncrement As Integer
        Property XElement As XElement
        Function IndexOf(Entity As T) As Integer
        Sub ResetCount()
        Sub ResetLastId()
        Sub ResetLastId(LastId As Integer)
        'Sub TransformAndValidate(Handler As ValidationEventHandler)
    End Interface
#End Region

#Region "<Classes>"
    Public Class EntityCommandObjectNullException(Of T As Entity) : Inherits Exception
#Region "<Variables>"
        Const MessageTemplate As String = "The 'EntityCommand' object for Entity type: {0}; could not be found."
#End Region

#Region "<Properties>"
        Public Property Entity As T
#End Region

#Region "<Constructors>"
        Public Sub New(Entity As T)
            MyBase.New(String.Format(MessageTemplate, GetType(T).FullName))
            Me.Entity = Entity
        End Sub

        Public Sub New(Entity As T, Message As String)
            MyBase.New(Message)
            Me.Entity = Entity
        End Sub

        Public Sub New(Entity As T, Message As String, InnerException As Exception)
            MyBase.New(Message, InnerException)
            Me.Entity = Entity
        End Sub
#End Region
    End Class

    Public Class EntityDuplicateException(Of T As Entity) : Inherits Exception
#Region "<Variables>"
        Const MessageTemplate As String = "The 'Entity' object for Entity type: {0}; is found to have a duplicate within the data source."
#End Region

#Region "<Properties>"
        Public Property Entity As T
#End Region

#Region "<Constructors>"
        Public Sub New(Entity As T)
            MyBase.New(String.Format(MessageTemplate, GetType(T).FullName))
            Me.Entity = Entity
        End Sub

        Public Sub New(Entity As T, Message As String)
            MyBase.New(Message)
            Me.Entity = Entity
        End Sub

        Public Sub New(Entity As T, Message As String, InnerException As Exception)
            MyBase.New(Message, InnerException)
            Me.Entity = Entity
        End Sub
#End Region
    End Class

    Public MustInherit Class Entity
#Region "<Methods>"
        Private Shared Function _CultureNeutralConvert(Of T As Structure)(value As Object) As T
            Dim _Thread = Thread.CurrentThread
            Dim _Culture = _Thread.CurrentCulture
            Dim _Result As T
            ' Convert
            _Thread.CurrentCulture = CultureInfo.InvariantCulture ' Ensure that all conversions are culture neutral.
            Try
                _Result = DirectCast(TypeDescriptor.GetConverter(GetType(T)).ConvertFromInvariantString(value), T)
            Catch
                Throw
            Finally
                _Thread.CurrentCulture = _Culture ' Ensure that the culture is returned back to its intended state.
            End Try
            Return _Result
        End Function

        ''' <summary>
        '''     Generic helper method that will parse Entity Objects into a specific nullable structured type.
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="value">Database Object containing data</param>
        Public Shared Function Parse(Of T As Structure)(value As Object) As T?
            ' Get the string equivalent of the supplied value.
            '   This is so that nullable structures will still work (yes, it turns out that nullable types are structure types, not reference types).
            Dim _value = value.ToString()
            If IsDBNull(value) OrElse String.IsNullOrEmpty(_value) Then
                Return Nothing
            End If
            Return New T?(_CultureNeutralConvert(Of T)(_value))
        End Function

        ''' <summary>
        '''     Helper method that will parse Entity Objects into a string (returns nothing if DbNull).
        ''' </summary>
        ''' <param name="value">Database Object containing data</param>
        Public Shared Function Parse(value As Object) As String
            If value Is Nothing OrElse IsDBNull(value) OrElse String.IsNullOrEmpty(value.ToString()) Then
                Return Nothing
            End If
            Return value.ToString()
        End Function

        ''' <summary>
        '''     Generic helper method that will parse Entity Objects into a specific structured type, initialising to default objects if null or empty.
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="value">Database Object containing data</param>
        Public Shared Function ParseOrDefault(Of T As Structure)(value As Object) As T
            If value Is Nothing OrElse IsDBNull(value) OrElse String.IsNullOrEmpty(value.ToString()) Then
                Return Activator.CreateInstance(Of T)()
            End If
            Return _CultureNeutralConvert(Of T)(value)
        End Function

        ''' <summary>
        '''     Helper method that will parse Entity Objects into a string (returns "String.Empty" if DbNull or empty).
        ''' </summary>
        ''' <param name="value">Database Object containing data</param>
        Public Shared Function ParseOrDefault(value As Object) As String
            If value Is Nothing OrElse IsDBNull(value) OrElse String.IsNullOrEmpty(value.ToString()) Then
                Return String.Empty
            End If
            Return value.ToString()
        End Function
#End Region
    End Class

    Public MustInherit Class EntityCommand
    End Class

    Public MustInherit Class EntityCommandFile : Inherits EntityCommand
    End Class

    Public MustInherit Class EntityCommandSoap : Inherits EntityCommand
    End Class

    Public MustInherit Class EntityCommandLog : Inherits EntityCommandFile
#Region "<Constants>"
        Public Const ObjectIdName = ""
#End Region
    End Class

    Public MustInherit Class EntityCommandXml : Inherits EntityCommand
#Region "<Constants>"
        Public Const ObjectIdName = ""
#End Region
    End Class

    Public MustInherit Class EntityCommandXmlDocument : Inherits EntityCommand
#Region "<Constants>"
        Public Const ObjectIdName = ""
#End Region
    End Class
#End Region
End Namespace

Namespace Models.ViewModels
    Public MustInherit Class ViewModel : Implements INotifyPropertyChanged
#Region "<Events>"
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
#End Region

#Region "<Methods>"
        Friend Sub _PropertyChanged(PropertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
        End Sub
#End Region
    End Class
End Namespace

Namespace Models.Xml
#Region "<Classes>"
    Public Class EntityNotFoundException(Of T As Entity) : Inherits Exception
#Region "<Variables>"
        Const MessageTemplate As String = "The 'Entity' object {0}; cannot be found within the data source."
#End Region

#Region "<Properties>"
        Public Property Entity As T
#End Region

#Region "<Constructors>"
        Public Sub New(Entity As T)
            MyBase.New(String.Format(MessageTemplate, Entity.ToString()))
            Me.Entity = Entity
        End Sub

        Public Sub New(Entity As T, Message As String)
            MyBase.New(Message)
            Me.Entity = Entity
        End Sub

        Public Sub New(Entity As T, Message As String, InnerException As Exception)
            MyBase.New(Message, InnerException)
            Me.Entity = Entity
        End Sub
#End Region
    End Class
#End Region
End Namespace