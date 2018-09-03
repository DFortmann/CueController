//
// Pandoras Box Automation SDK for C#.Net
// coolux® GmbH - http://www.coolux.de
//
// 28.02.2014
//

using System;
using System.Runtime.InteropServices;

enum AutoError {
None = 0,
NoConnection = 1,
WrongParam = 2,
AddressTranslation = 3,
CouldNotConnectToSocket = 4,
HandshakeFailed = 5,
RequestTimedOut = 6,
WrongMessageReturned = 7,
ParamPointer = 8,
WrongClient = 9,
HostInvalidLayer = 10,
HostInvalidSequence = 11,
HostInvalidPointer = 12,
HostInvalidParameterName = 13,
HostInvalidParam = 14,
InvalidPort = 15,
WrongNetworkProtocol = 16,
AlreadyConnected = 17,
InvalidCueId = 18,
InvalidCueButtonId = 19,
InvalidDomainNr = 20,
GraphicLayerNotCreated = 21,
InvalidSiteId = 22,
InvalidViewId = 23,
InvalidCast = 24,
AddingVideoLayerNotAllowed = 25,
InvalidLayerMoveTarget = 26,
InvalidFolderPath = 27,
DmxResourceNotFound = 28,
NoAdditionalSequenceAllowed = 29,
InvalidContentPath = 30,
HandshakeTimeout = 31,
FunctionNotSupportedByOS = 32,
TreeItemIndexNoMediaFile = 33,
TreeItemNotFound = 34,
InvalidTreeItemIndex = 35,
NoThumbnailAvailable = 36,
EncryptionKeyNotValid = 37,
EncryptionPolicyNotValid = 38,
NoEncryptionManager = 39,
}
    
    enum SequenceTimeCodeMode : int {
            
        None = 0,
            
        Send = 1,
            
        Receive = 2,
    }
        
    enum SequenceTimeCodeStopAction : int {
            
        None = 0,
            
        Stop = 1,
            
        Pause = 2,
            
        Continue = 3,
    }
        
    enum TransportMode : int {
            
        Stop = 0,
            
        Pause = 128,
            
        Play = 64,
            
        PlayLoop = 192,
    }
        
    enum CuePlayMode {
            
        Play = 0,
            
        Pause = 1,
            
        Stop = 2,
            
        Jump = 3,
            
        Wait = 4,
    }
    
    public struct TimeType {
        
        public int VersionNum;
        
        public int Hours;
        
        public int Minutes;
        
        public int Seconds;
        
        public int Frames;
    }
    
    public struct MediaOptionsType {
        
        public bool anisotropicFiltering;
        
        public bool ignoreThumbnail;
        
        public bool alphaChannel;
        
        public bool fluidFrame;
        
        public bool optimizeMpegColorspace;
        
        public bool underscan;
        
        public bool optimizeLooping;
        
        public bool muteSound;
    }
    
    public struct MediaType {
        
        public int VersionNum;
        
        public int dmxId;
        
        public int dmxFolderId;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=64)]
        public byte[] name;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=128)]
        public byte[] path;
        
        public int width;
        
        public int height;
        
        public int fps;
        
        public TimeType Length;
        
        public MediaOptionsType options;
    }

    public struct MediaType1
    {

        public int dmxId;

        public int dmxFolderId;

        public string path;

        public string projectPath;

        public int width;

        public int height;

        public int fps;

        public TimeType Length;

        public MediaOptionsType options;
    }

    internal struct _MediaType1
    {

        public int dmxId;

        public int dmxFolderId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] path;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] projectPath;

        public int width;

        public int height;

        public int fps;

        public TimeType Length;

        public MediaOptionsType options;
    }
    public struct LayerType {
        
        public int VersionNum;
        
        public int siteNum;
        
        public int deviceNum;
    }
    
    public struct ParamResourceType {
        
        public int folderId;
        
        public int fileId;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1024)]
        public byte[] path;
    }
    
    public struct ParamResourceType1 {
        
        public int folderId;
        
        public int fileId;
        
        public string path;
        
        public string projectPath;
    }

    internal struct _ParamResourceType1
    {

        public int folderId;

        public int fileId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] path;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] projectPath;
    }

    public struct TreeItemType
    {

        public string projectPath;

        public string idPath;

        public int type;
    }
    internal struct _TreeItemType
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] projectPath;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] idPath;

        public int type;
    }

    class Auto
    {

    /// <summary>
    /// Initializes the connection to a Pandoras Box Master System using UDP
    /// </summary>
    /// <param name="IpStr">IP Address of the machine running PB-Master</param>
    /// <param name="domain">Domain ID</param>
    /// <returns>True: No error ocurred
    /// False: Error ocurred</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll",EntryPoint="AutoInitialize")]
    public static extern bool Initialize(string IpStr, int domain);
    
    /// <summary>
    /// Initializes the connection to a Pandoras Box Master System using TCP
    /// </summary>
    /// <param name="IpStr">IP Address of the machine running PB-Master</param>
    /// <param name="domain">Domain ID</param>
    /// <param name="waitForConnection">True: start a background thread to wait for a new connection
    /// False: only try once to connect</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoInitializeTCP")]
    public static extern bool InitializeTCP(string IpStr, int domain, bool waitForConnection);
    
    /// <summary>
    /// Close any open connections
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoUnInitialize")]
    public static extern bool UnInitialize();
    
    /// <summary>
    /// Starts a thread that keeps trying to connect to the PB-Master
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoWaitForConnection")]
    public static extern bool WaitForConnection();
    
    /// <summary>
    /// Stops the thread that keeps trying to connect to the PB-Master
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoStopWaitingForConnection")]
    public static extern bool StopWaitingForConnection();
    
    /// <summary>
    /// Gets the current connection state
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetIsConnected")]
    public static extern bool GetIsConnected();
    
    /// <summary>
    /// Gets the code for the last error that occurred
    /// </summary>
    /// <returns>Error code</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetLastError")]
    public static extern AutoError GetLastError();
    
    /// <summary>
    /// Set a parameter to a specific value
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="value">The value will be interpreted </param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamDouble")]
    public static extern bool SetParamDouble(int siteNum, int deviceNum, string ParamName, double value);
    
    /// <summary>
    /// Set a parameter to a specific value
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="value">Value</param>
    /// <param name="silent">Do not mark parameter active</param>
    /// <param name="direct">Do not use transition smoothing</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamDoubleExtended")]
    public static extern bool SetParamDoubleExtended(int siteNum, int deviceNum, string ParamName, double value, bool silent, bool direct);
    
    /// <summary>
    /// Sets the given parameter to given value for all layers in the current selection
    /// </summary>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="value">The value will be interpreted</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamInSelectionDouble")]
    public static extern bool SetParamInSelectionDouble(string ParamName, double value);
    
    /// <summary>
    /// Gets the value of the parameter for given site/device
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetParam")]
    public static extern double GetParam(int siteNum, int deviceNum, string ParamName);
    
    /// <summary>
    /// Sets the media for the container found at the given Site/Device/Sequence/Time combination.
    /// Note: This function will neither create new containers nor will it add keys. It only works with existing containers.
    /// </summary>
    /// <param name="siteNum">Target Site ID</param>
    /// <param name="deviceNum">Target Device ID</param>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <param name="dmxFolderId">Media Folder ID</param>
    /// <param name="dmxId">Media ID</param>
    /// <returns>Returns true if the PB-Master received the command</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetContentAtTime")]
    public static extern bool SetContentAtTime(int siteNum, int deviceNum, int seqNum, int hours, int minutes, int seconds, int frames, int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Assign a Mesh to given site/device identified by DmxFolder and DmxId
    ///  </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAssignMesh")]
    public static extern bool AssignMesh(int siteNum, int deviceNum, int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Assign a Mesh to given site/device identified by name. Also allows to assign eeshes to effects etc. using ParamName
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="MeshName">Mesh Name</param>
    /// <param name="ParamName">The parameter to assign the mesh to. Use "Mesh" to assign to the device itself</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAssignMeshByName")]
    public static extern bool AssignMeshByName(int siteNum, int deviceNum, string MeshName, string ParamName);
    
    /// <summary>
    /// Assign a Mesh to selected devices
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAssignMeshToSelection")]
    public static extern bool AssignMeshToSelection(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Assign Media to given site/device identified by DmxFolder and DmxId
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAssignMedia")]
    public static extern bool AssignMedia(int siteNum, int deviceNum, int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Assign Media to given site/device identified by name
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="MediaName">Media Name</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAssignMediaByName")]
    public static extern bool AssignMediaByName(int siteNum, int deviceNum, string MediaName, string ParamName);
    
    /// <summary>
    /// Assign Media to selected devices
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAssignMediaToSelection")]
    public static extern bool AssignMediaToSelection(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Moves content identified by name to given folder
    /// </summary>
    /// <param name="ContentName">The path+name of the content in the projects tab. ex.: Myfolder/mySubfolder/somecontent.mpg</param>
    /// <param name="FolderName">Path to move to. ex.: SomeFolder/MyTargetFolder</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveContentToFolder")]
    public static extern bool MoveContentToFolder(string ContentName, string FolderName);
    
    /// <summary>
    /// Move tree item to another tree item
    /// </summary>
    /// <param name="itemIdFrom">tree item source</param>
    /// <param name="itemIdTo">tree item target</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveTreeItem")]
    public static extern bool MoveTreeItem(int itemIdFrom, int itemIdTo);
    
    /// <summary>
    /// Sets the transport mode for sequence with given ID
    /// </summary>
    /// <param name="sequenceNum">Sequence ID</param>
    /// <param name="ModeName">Transport mode, case sensitive (Play/Pause/Stop)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceTransportMode")]
    public static extern bool SetSequenceTransportMode(int sequenceNum, string ModeName);
    
    /// <summary>
    /// Moves the nowpointer to the position of the cue with given ID
    /// </summary>
    /// <param name="sequenceNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveSequenceToCue")]
    public static extern bool MoveSequenceToCue(int sequenceNum, int cueId);
    
    /// <summary>
    /// Moves the nowpointer to given time
    /// </summary>
    /// <param name="sequenceNum">Sequence ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveSequenceToTime")]
    public static extern bool MoveSequenceToTime(int sequenceNum, int hours, int minutes, int seconds, int frames);
    
    /// <summary>
    /// Moves to either the next/previous frame
    /// </summary>
    /// <param name="sequenceNum">Sequence ID</param>
    /// <param name="isNext">true: next frame | false: previous frame</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveSequenceToLastNextFrame")]
    public static extern bool MoveSequenceToLastNextFrame(int sequenceNum, bool isNext);
    
    /// <summary>
    /// Moves to either the next/previous cue
    /// </summary>
    /// <param name="sequenceNum">Sequence ID</param>
    /// <param name="isNext">true: next cue | false: previous cue</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveSequenceToLastNextCue")]
    public static extern bool MoveSequenceToLastNextCue(int sequenceNum, bool isNext);
    
    /// <summary>
    /// Sets the transparency for given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="transparency">Opacity between 0 and 255</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceTransparency")]
    public static extern bool SetSequenceTransparency(int seqNum, int transparency);
    
    /// <summary>
    /// Get the transparency of given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <returns>Sequence Transparency (0-255)</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetSequenceTransparency")]
    public static extern bool GetSequenceTransparency(int seqNum);
    
    /// <summary>
    /// Enables or disables a sequence to send/receive timecode
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="timeCodeMode">The timecode mode to use</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceTimeCodeMode")]
    public static extern bool SetSequenceTimeCodeMode(int seqNum, SequenceTimeCodeMode timeCodeMode);
    
    /// <summary>
    /// Set the offset that will be added to the Pandoras Box Timecode. Negative values possible.
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceTimeCodeOffset")]
    public static extern bool SetSequenceTimeCodeOffset(int seqNum, int hours, int minutes, int seconds, int frames);
    
    /// <summary>
    /// Set the behavior on timecode signal stop
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="stopAction">stop,pause or continue playback</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceTimeCodeStopAction")]
    public static extern bool SetSequenceTimeCodeStopAction(int seqNum, SequenceTimeCodeStopAction stopAction);
    
    /// <summary>
    /// Reset all active values
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoResetAll")]
    public static extern bool ResetAll();
    
    /// <summary>
    /// Reset all active values for given site
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoResetSite")]
    public static extern bool ResetSite(int siteNum);
    
    /// <summary>
    /// Reset all active values for given device
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoResetDevice")]
    public static extern bool ResetDevice(int siteNum, int deviceNum);
    
    /// <summary>
    /// Remove active value of specific parameter
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoResetParam")]
    public static extern bool ResetParam(int siteNum, int deviceNum, string ParamName);
    
    /// <summary>
    /// Select all parameters as active
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoActivateAll")]
    public static extern bool ActivateAll();
    
    /// <summary>
    /// Set all parameters of all devices of a whole site as active
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoActivateSite")]
    public static extern bool ActivateSite(int siteNum);
    
    /// <summary>
    /// Set all parameters of a specific device as active
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoActivateDevice")]
    public static extern bool ActivateDevice(int siteNum, int deviceNum);
    
    /// <summary>
    /// Set a specific parameter as active
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoActivateParam")]
    public static extern bool ActivateParam(int siteNum, int deviceNum, string ParamName);
    
    /// <summary>
    /// Set all active values as inactive. The values themselves are preserved.
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoClearAllActive")]
    public static extern bool ClearAllActive();
    
    /// <summary>
    /// Set all active values of a site as inactive. The values themselves are preserved.
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoClearActiveSite")]
    public static extern bool ClearActiveSite(int siteNum);
    
    /// <summary>
    /// Set all active values of a specific device as inactive. The values themselves are preserved.
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoClearActiveDevice")]
    public static extern bool ClearActiveDevice(int siteNum, int deviceNum);
    
    /// <summary>
    /// Set a parameter value inactive. The values themselves are preserved.
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoClearActiveParam")]
    public static extern bool ClearActiveParam(int siteNum, int deviceNum, string ParamName);
    
    /// <summary>
    /// Toggles full screen mode of a site
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoToggleFullscreen")]
    public static extern bool ToggleFullscreen(int siteNum);
    
    /// <summary>
    /// Add or subtract a value from a parameter
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="value">Value to add, can be negative</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamRelativeDouble")]
    public static extern bool SetParamRelativeDouble(int siteNum, int deviceNum, string ParamName, double value);
    
    /// <summary>
    /// Add or subtract a value from a parameter
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="value">Value to add, can be negative</param>
    /// <param name="silent">Do not mark parameter active</param>
    /// <param name="direct">Do not use transition smoothing</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamRelativeDoubleExtended")]
    public static extern bool SetParamRelativeDoubleExtended(int siteNum, int deviceNum, string ParamName, double value, bool silent, bool direct);
    
    /// <summary>
    /// Add or subtract a value from a parameter in the current selection
    /// </summary>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="value">Value to add, can be negative</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamRelativeInSelectionDouble")]
    public static extern bool SetParamRelativeInSelectionDouble(string ParamName, double value);
    
    /// <summary>
    /// Add content from given path and assign DmxIDs
    /// </summary>
    /// <param name="FullPath">Absolute system path to media to add (ex. c:/coolux/content/...)</param>
    /// <param name="siteNum">Site ID</param>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContent")]
    public static extern bool AddContent(string FullPath, int siteNum, int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Adds content from given folder to a specific folder, also assigning DmxIDs
    /// </summary>
    /// <param name="FullPath">Absolute system path to media to add (ex. c:/coolux/content/...)</param>
    /// <param name="siteNum">Site ID</param>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="Foldername">Target folder to add content to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentToFolder")]
    public static extern bool AddContentToFolder(string FullPath, int siteNum, int dmxFolderId, int dmxId, string Foldername);
    
    /// <summary>
    /// Add content to a tree item
    /// </summary>
    /// <param name="FullPath">Absolute system path to media to add (ex. c:/coolux/content/...)</param>
    /// <param name="siteNum">Site ID</param>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="treeItemId">Target tree item</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentToTreeItem")]
    public static extern bool AddContentToTreeItem(string FullPath, int siteNum, int dmxFolderId, int dmxId, int treeItemId);
    
    /// <summary>
    /// Add content from local node using an absolute path
    /// </summary>
    /// <param name="FullPath">Absolute system path to media to add (ex. c:/coolux/content/...)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFromLocalNode")]
    public static extern bool AddContentFromLocalNode(string FullPath);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="FullPath">Absolute system path to media to add (ex. c:/coolux/content/...)</param>
    /// <param name="Foldername">Target path in PB project</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFromLocalNodeToFolder")]
    public static extern bool AddContentFromLocalNodeToFolder(string FullPath, string Foldername);
    
    /// <summary>
    /// Add content from an absolute system path to a specific tree node
    /// </summary>
    /// <param name="FullPath">Absolute system path to media to add (ex. c:/coolux/content/...)</param>
    /// <param name="treeItemId">target tree item</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFromLocalNodeToTreeItem")]
    public static extern bool AddContentFromLocalNodeToTreeItem(string FullPath, int treeItemId);
    
    /// <summary>
    /// Add a complete folder with content to the project
    /// </summary>
    /// <param name="FolderPath">Path to content</param>
    /// <param name="siteNum">Site ID</param>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="ProjectPath">Project path</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFolder")]
    public static extern bool AddContentFolder(string FolderPath, int siteNum, int dmxFolderId, int dmxId, string ProjectPath);
    
    /// <summary>
    /// Add a complete folder with content to the projecta
    /// </summary>
    /// <param name="FolderPath">Folder to add</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFolderFromLocalNode")]
    public static extern bool AddContentFolderFromLocalNode(string FolderPath);
    
    /// <summary>
    /// Add a complete folder with content to the project at a specific folder
    /// </summary>
    /// <param name="FolderPath">Folder to add</param>
    /// <param name="Foldername">Target folder in project tree</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFolderFromLocalNodeToFolder")]
    public static extern bool AddContentFolderFromLocalNodeToFolder(string FolderPath, string Foldername);
    
    /// <summary>
    /// Add a complete folder with content to the project to a specific TreeItem
    /// </summary>
    /// <param name="FolderPath">Folder to add</param>
    /// <param name="treeItemId">tree item to add to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddContentFolderFromLocalNodeToTreeItem")]
    public static extern bool AddContentFolderFromLocalNodeToTreeItem(string FolderPath, int treeItemId);
    
    /// <summary>
    /// Removes Media with given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveMediaById")]
    public static extern bool RemoveMediaById(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Removes Mesh with given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveMeshById")]
    public static extern bool RemoveMeshById(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Removes content by project path
    /// </summary>
    /// <param name="ProjectPath">Path to the project content</param>
    /// <param name="allEquallyNamed">True: Removes all contents with the same name| False: Remove only one (the first) content with that name</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveContentByName")]
    public static extern bool RemoveContentByName(string ProjectPath, bool allEquallyNamed);
    
    /// <summary>
    /// Remove a tree item
    /// </summary>
    /// <param name="treeItemId">tree item to remove</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveTreeItem")]
    public static extern bool RemoveTreeItem(int treeItemId);
    
    /// <summary>
    /// Remove all resources in specific folder
    /// </summary>
    /// <param name="removeFolder">True: Removes everything | False: Remove files only. Folder structure stays intact</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveAllResources")]
    public static extern bool RemoveAllResources(bool removeFolder);
    
    /// <summary>
    /// Trigger a "Spread All Resources"
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSpreadAll")]
    public static extern bool SpreadAll();
    
    /// <summary>
    /// Trigger spread for media identified by DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSpreadMediaById")]
    public static extern bool SpreadMediaById(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Trigger spread for mesh identified by DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSpreadMeshById")]
    public static extern bool SpreadMeshById(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Trigger reload for media with given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoReloadMediaById")]
    public static extern bool ReloadMediaById(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Trigger reload for mesh with given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoReloadMeshById")]
    public static extern bool ReloadMeshById(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Trigger reload for resource with given name
    /// </summary>
    /// <param name="ProjectPath">project path</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoReloadResource")]
    public static extern bool ReloadResource(string ProjectPath);
    
    /// <summary>
    /// Trigger spread for resource with given name
    /// </summary>
    /// <param name="ProjectPath">project path</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSpreadResource")]
    public static extern bool SpreadResource(string ProjectPath);
    
    /// <summary>
    /// Trigger reload and spread for resource identified by path name
    /// </summary>
    /// <param name="ProjectPath">project path</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoReloadAndSpreadResourceByPath")]
    public static extern bool ReloadAndSpreadResourceByPath(string ProjectPath);
    
    /// <summary>
    /// Trigger reload and spread for resource identified by tree item index
    /// </summary>
    /// <param name="treeItemId">tree item id of resource</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoReloadAndSpreadResourceByItemIndex")]
    public static extern bool ReloadAndSpreadResourceByItemIndex(int treeItemId);
    
    /// <summary>
    /// Trigger reload and spread for resource identified by DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoReloadAndSpreadResourceByDmxId")]
    public static extern bool ReloadAndSpreadResourceByDmxId(int dmxfolderId, int dmxId);
    
    /// <summary>
    /// Remove all inconsistent files
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveInconsistent")]
    public static extern bool RemoveInconsistent();
    
    /// <summary>
    /// Stores active values in given sequence at the current timecode
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoStoreActive")]
    public static extern bool StoreActive(int seqNum);
    
    /// <summary>
    /// Stores active values in given sequence at given time
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoStoreActiveToTime")]
    public static extern bool StoreActiveToTime(int seqNum, int hours, int minutes, int seconds, int frames);
    
    /// <summary>
    /// Sets the Frame Blending for media identified by given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="frameBlended">Use fra</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetMediaFrameBlendingById")]
    public static extern bool SetMediaFrameBlendingById(int dmxFolderId, int dmxId, bool frameBlended);
    
    /// <summary>
    /// Sets the Deinterlacing for media identified by given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="deinterlacer">Use deinterlacing?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetMediaDeinterlacingById")]
    public static extern bool SetMediaDeinterlacingById(int dmxFolderId, int dmxId, int deinterlacer);
    
    /// <summary>
    /// Sets the Anisostropic Filtering for media identified by given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="useFiltering">Use Anisostrophicfiltering?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetMediaAnisotropicFilteringById")]
    public static extern bool SetMediaAnisotropicFilteringById(int dmxFolderId, int dmxId, bool useFiltering);
    
    /// <summary>
    /// Sets the Underscan for media identified by given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="useUnderscan">Use underscan?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetMediaUnderscanById")]
    public static extern bool SetMediaUnderscanById(int dmxFolderId, int dmxId, bool useUnderscan);
    
    /// <summary>
    /// Sets wether to use mpeg color space conversion for media identified by given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="useMpegColourSpace">Use mpeg color space conversion?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetMediaMpegColourSpaceById")]
    public static extern bool SetMediaMpegColourSpaceById(int dmxFolderId, int dmxId, bool useMpegColourSpace);
    
    /// <summary>
    /// Sets the Alpha Channel usage for media identified by given DmxID
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="useAlphaChannel">Use alpha channel?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetMediaAlphaChannelById")]
    public static extern bool SetMediaAlphaChannelById(int dmxFolderId, int dmxId, bool useAlphaChannel);
    
    /// <summary>
    /// Creates a new text asset with given DmxIDs
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="Text">Asset text</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoCreateTextInput")]
    private static extern bool _CreateTextInput(int dmxFolderId, int dmxId, string Text);
    
    /// <summary>
    /// Sets the text for the text asset with given DmxIDs
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="Text">Text</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetText")]
    public static extern bool SetText(int dmxFolderId, int dmxId, string Text);
    
    /// <summary>
    /// Loads a project from given path with given filename.
    /// </summary>
    /// <param name="Path">Folder name</param>
    /// <param name="Name">Project file name</param>
    /// <param name="saveExisting">Save the existing project before opening the new one?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoLoadProject")]
    public static extern bool LoadProject(string Path, string Name, bool saveExisting);
    
    /// <summary>
    /// Closes the current project
    /// </summary>
    /// <param name="save">Save project before close?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoCloseProject")]
    public static extern bool CloseProject(bool save);
    
    /// <summary>
    /// Clears the device selection
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoClearSelection")]
    public static extern bool ClearSelection();
    
    /// <summary>
    /// Set wether given device should accept Dmx values
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="acceptDmx">Accept Dmx?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetDeviceAcceptDmxById")]
    public static extern bool SetDeviceAcceptDmxById(int siteNum, int deviceNum, bool acceptDmx);
    
    /// <summary>
    /// Set wether given site should accept Dmx values
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="acceptDmx">Accept Dmx?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSiteAcceptDmxById")]
    public static extern bool SetSiteAcceptDmxById(int siteNum, bool acceptDmx);
    
    /// <summary>
    /// Sets the Dmx address for given site/device
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="index">Channel Id</param>
    /// <param name="id1">Dmx Subnet</param>
    /// <param name="id2">Dmx Universe</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetDeviceDmxAddressById")]
    public static extern bool SetDeviceDmxAddressById(int siteNum, int deviceNum, int index, int id1, int id2);
    
    /// <summary>
    /// Sets the play mode for a cue at given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <param name="playMode">The cue play mode</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceCuePlayMode")]
    public static extern bool SetSequenceCuePlayMode(int seqNum, int cueId, CuePlayMode playMode);
    
    /// <summary>
    /// Sets the play mode for the next cue at given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="playMode">The cue play mode</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetNextSequenceCuePlayMode")]
    public static extern bool SetNextSequenceCuePlayMode(int seqNum, int playMode);
    
    /// <summary>
    /// Sets wether to ignore the next cue in given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="doIgnore">Ignore next cue?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetIgnoreNextSequenceCue")]
    public static extern bool SetIgnoreNextSequenceCue(int seqNum, bool doIgnore);
    
    //  TODO
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetChannelEvents")]
    public static extern bool SetChannelEvents(int ctEvents, ref int Events);
    
    /// <summary>
    /// Saves the current project
    /// </summary>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSaveProject")]
    public static extern bool SaveProject();
    
    /// <summary>
    /// Change the fullscreen mode for given site identified by Id
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="enterFullscreen">Enter fullscreen?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoChangeFullscreenStateById")]
    public static extern bool ChangeFullscreenStateById(int siteNum, bool enterFullscreen);
    
    /// <summary>
    /// Change the fullscreen mode for given site identified by Ip
    /// </summary>
    /// <param name="Ip">IP Address</param>
    /// <param name="enterFullscreen">Enter fullscreen?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoChangeFullscreenStateByIp")]
    public static extern bool ChangeFullscreenStateByIp(string Ip, bool enterFullscreen);
    
    /// <summary>
    /// Sets the texture size for text asset identified by DmxId
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="width">New texture width</param>
    /// <param name="height">New texture height</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetTextTextureSize")]
    public static extern bool SetTextTextureSize(int dmxFolderId, int dmxId, int width, int height);
    
    /// <summary>
    /// Set the style for an existing text asset
    /// </summary>
    /// <param name="dmxFolderId">Dmx ID (Folder)</param>
    /// <param name="dmxId">Dmx ID (Item)</param>
    /// <param name="Font">Font name</param>
    /// <param name="size">size in pixels</param>
    /// <param name="style">text style</param>
    /// <param name="alignment">text alignment</param>
    /// <param name="colorRed">Color (Red)</param>
    /// <param name="colorGreen">Color (Green)</param>
    /// <param name="colorBlue">Color (Blue)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetTextProperties")]
    public static extern bool SetTextProperties(int dmxFolderId, int dmxId, string Font, int size, int style, int alignment, int colorRed, int colorGreen, int colorBlue);
    
    /// <summary>
    /// Set the center on texture for text asset identified by DmxId
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="centerOnTexture">Center text on texture?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetTextCenterOnTexture")]
    public static extern bool SetTextCenterOnTexture(int dmxFolderId, int dmxId, bool centerOnTexture);
    
    /// <summary>
    /// Creates new text asset. Adjusts width automatically.
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="Text">Text</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>Wrapper Function</remarks>
    public static bool CreateTextInputWide(int dmxFolderId, int dmxId, string Text)
    {
        byte[] bText = System.Text.Encoding.Unicode.GetBytes(Text);
        return _CreateTextInputWide(dmxFolderId, dmxId, ref bText[0]);
    }
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoCreateTextInputWide")]
    private static extern bool _CreateTextInputWide(int dmxFolderId, int dmxId, ref byte Text);
    
    /// <summary>
    /// Sets the text of a text asset and adjusts width automatically
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <param name="Text">Text</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>Wrapper Function</remarks>
    public static bool SetTextWide(int dmxFolderId, int dmxId, string Text)
    {
        byte[] bText = System.Text.Encoding.Unicode.GetBytes(Text);
        return _SetTextWide(dmxFolderId, dmxId, ref bText[0]);
    }
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetTextWide")]
    private static extern bool _SetTextWide(int dmxFolderId, int dmxId, ref byte Text);
    
    /// <summary>
    /// Changes the IP address of a site.
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="Ip">IP Address</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSiteIpById")]
    public static extern bool SetSiteIpById(int siteNum, string Ip);
    
    /// <summary>
    /// Check if layer is in current selection
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Is the given layer currently selected?</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoIsLayerReallySelected")]
    public static extern bool IsLayerReallySelected(int siteNum, int deviceNum);
    
    /// <summary>
    /// Gets the number of media in the project
    /// </summary>
    /// <returns>Number of media in project</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetNumMediaInProject")]
    public static extern bool GetNumMediaInProject();
    
    /// <summary>
    /// Gets the number of tree items in project
    /// </summary>
    /// <returns>Number of tree items in project</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetNumTreeItemsInProject")]
    public static extern bool GetNumTreeItemsInProject();
    
    
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetMediaInfo1")]
    private static extern bool _GetMediaInfo1(int index, ref _MediaType1 MediaInfo);

    /// <summary>
    /// Gets information on media identified by index
    /// </summary>
    /// <param name="index">Index</param>
    /// <param name="MediaInfo">MediaInfo1 object to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>This is a wrapper function!</remarks>
    public static bool GetMediaInfo1(int index, ref MediaType1 MediaInfo){
        _MediaType1 _mt1 = new _MediaType1();
        bool ret = _GetMediaInfo1(index,ref _mt1);
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        MediaInfo.dmxFolderId = _mt1.dmxFolderId;
        MediaInfo.dmxId = _mt1.dmxId;
        MediaInfo.fps = _mt1.fps;
        MediaInfo.height = _mt1.height;
        MediaInfo.Length = _mt1.Length;
        MediaInfo.options = _mt1.options;
        MediaInfo.width = _mt1.width;
        MediaInfo.path = enc.GetString(_mt1.path).TrimEnd(new char[] { '\0' });
        MediaInfo.projectPath = enc.GetString(_mt1.projectPath).TrimEnd(new char[] { '\0' });
        return ret;
    }
    
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetMediaInfoFromTreeItem")]
    private static extern bool _GetMediaInfoFromTreeItem(int treeItemIndex, ref _MediaType1 MediaInfo);

    /// <summary>
    /// Gets information on media identified by index
    /// </summary>
    /// <param name="treeItemindex">Index</param>
    /// <param name="MediaInfo">MediaInfo1 object to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    public static bool GetMediaInfoFromTreeItem(int index, ref MediaType1 MediaInfo)
    {
        _MediaType1 _mt1 = new _MediaType1();
        bool ret = _GetMediaInfoFromTreeItem(index, ref _mt1);
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        MediaInfo.dmxFolderId = _mt1.dmxFolderId;
        MediaInfo.dmxId = _mt1.dmxId;
        MediaInfo.fps = _mt1.fps;
        MediaInfo.height = _mt1.height;
        MediaInfo.Length = _mt1.Length;
        MediaInfo.options = _mt1.options;
        MediaInfo.width = _mt1.width;
        MediaInfo.path = enc.GetString(_mt1.path).TrimEnd(new char[] { '\0' });
        MediaInfo.projectPath = enc.GetString(_mt1.projectPath).TrimEnd(new char[] { '\0' });
        return ret;
    }

    /// <summary>
    /// Get information on tree item identified by index
    /// </summary>
    /// <param name="index">Index</param>
    /// <param name="ItemInfo">TreeItemType object to write results to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetTreeItemInfo")]
    private static extern bool _GetTreeItemInfo(int index, ref _TreeItemType ItemInfo);

    public static bool GetTreeItemInfo(int index, ref TreeItemType ItemInfo)
    {
        _TreeItemType _tit = new _TreeItemType();
        bool ret = _GetTreeItemInfo(index,ref _tit);
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        ItemInfo.idPath = enc.GetString(_tit.idPath).TrimEnd(new char[] {'\0'});
        ItemInfo.projectPath = enc.GetString(_tit.projectPath).TrimEnd(new char[] { '\0' });
        ItemInfo.type = _tit.type;
        return ret;
    }
    
    /// <summary>
    /// Get the transport mode of a sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetSequenceTransportMode")]
    public static extern TransportMode GetSequenceTransportMode(int seqNum);
    
    /// <summary>
    /// Get the time of a sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="Time">TimeType to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetSequenceTime")]
    public static extern bool GetSequenceTime(int seqNum, ref TimeType Time);
    
    /// <summary>
    /// Get the remaining time for a clip on a specific layer for given sequence
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="Time">TimeType to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetClipRemainingTime")]
    public static extern bool GetClipRemainingTime(int siteNum, int deviceNum, int seqNum, ref TimeType Time);
    
    /// <summary>
    /// Get remaining time until the next cue for given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="Time">TimeType to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetRemainingTimeUntilNextCue")]
    public static extern bool GetRemainingTimeUntilNextCue(int seqNum, ref TimeType Time);
    
    /// <summary>
    /// Get the number of selected layers
    /// </summary>
    /// <returns>Number of selected layers</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetNumSelectedLayers")]
    public static extern bool GetNumSelectedLayers();
    
    /// <summary>
    /// Get the selected layer with given number in selection.
    /// </summary>
    /// <param name="layerIndex">Index of the layer in selection. Values between 0 and (AutoGetNumSelectedLayers - 1)</param>
    /// <param name="layerInfo">LayerType object to write results to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetSelectedLayer")]
    public static extern bool GetSelectedLayer(int layerIndex, ref LayerType layerInfo);
    
    /// <summary>
    /// Create a new folder
    /// </summary>
    /// <param name="Name">Name for the new folder</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddFolderToProject")]
    public static extern bool AddFolderToProject(string Name);
    
    /// <summary>
    /// Create new folder in given path
    /// </summary>
    /// <param name="Name">Name for the new folder</param>
    /// <param name="FolderRoot">Path to create folder in</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddFolderToProjectPath")]
    public static extern bool AddFolderToProjectPath(string Name, string FolderRoot);
    
    /// <summary>
    /// Create new folder in given tree item
    /// </summary>
    /// <param name="Name">Name for the new folder</param>
    /// <param name="treeItemId">Tree item to create folder in</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddFolderToTreeItem")]
    public static extern bool AddFolderToTreeItem(string Name, int treeItemId);
    
    /// <summary>
    /// Remove a folder from the project
    /// </summary>
    /// <param name="FolderPath">Folder with path</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveFolderFromProject")]
    public static extern bool RemoveFolderFromProject(string FolderPath);
    
    /// <summary>
    /// Select / Deselect a device
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="selectionMode">True: Selected| False: Not selected</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetDeviceSelection")]
    public static extern bool SetDeviceSelection(int siteNum, int deviceNum, int selectionMode);
    
    /// <summary>
    /// Map a coolux controller fader to a sequence
    /// </summary>
    /// <param name="faderId">Fader ID</param>
    /// <param name="seqNum">Sequence ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetClxControllerFaderMapping")]
    public static extern bool SetClxControllerFaderMapping(int faderId, int seqNum);
    
    /// <summary>
    /// Map a coolux controller button to a cue
    /// </summary>
    /// <param name="cueBtnId">Button ID</param>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetClxControllerCueMapping")]
    public static extern bool SetClxControllerCueMapping(int cueBtnId, int seqNum, int cueId);
    
    /// <summary>
    /// Add cue to given sequence at given time
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <param name="Name">Cue name</param>
    /// <param name="cueKindId">Kind of cue</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddCue")]
    public static extern bool AddCue(int seqNum, int cueId, int hours, int minutes, int seconds, int frames, string Name, CuePlayMode cueKindId);
    
    /// <summary>
    /// Remove cue with given ID on given sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveCueById")]
    public static extern bool RemoveCueById(int seqNum, int cueId);
    
    /// <summary>
    /// Remove all cues for a sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveAllCues")]
    public static extern bool RemoveAllCues(int seqNum);
    
    /// <summary>
    /// Add new graphic layer
    /// </summary>
    /// <param name="siteId">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddGraphicLayer")]
    public static extern bool AddGraphicLayer(int siteId);
    
    /// <summary>
    /// Add new video layer
    /// </summary>
    /// <param name="siteId">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddVideoLayer")]
    public static extern bool AddVideoLayer(int siteId);
    
    /// <summary>
    /// Remove a graphic layer by id
    /// </summary>
    /// <param name="siteId">Site ID</param>
    /// <param name="layerId">Layer ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveGraphicLayer")]
    public static extern bool RemoveGraphicLayer(int siteId, int layerId);
    
    /// <summary>
    /// Remove a video layer by id
    /// </summary>
    /// <param name="siteId">Site ID</param>
    /// <param name="layerId">Layer ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveVideoLayer")]
    public static extern bool RemoveVideoLayer(int siteId, int layerId);
    
    /// <summary>
    /// Enables/Disables the backup mode
    /// </summary>
    /// <param name="enable">Enable backup mode?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoBackupMode")]
    public static extern bool BackupMode(bool enable);
    
    /// <summary>
    /// Applies view identified by given number
    /// </summary>
    /// <param name="viewNum">The view number</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoApplyView")]
    public static extern bool ApplyView(int viewNum);
    
    /// <summary>
    /// Set the spare from spread option
    /// </summary>
    /// <param name="siteId">Site ID</param>
    /// <param name="spareFromSpread">True: Do not spread resources to this site | False: Spreading resources possible</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSpareFromSpread")]
    public static extern bool SetSpareFromSpread(int siteId, bool spareFromSpread);
    
    
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetParamMedia1")]
    private static extern bool _GetParamMedia1(int siteNum, int deviceNum, string ParamName, ref _ParamResourceType1 Info);

    /// <summary>
    /// Get resource information of a device
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="Info">ResourceType1 object to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks></remarks>
    public static bool GetParamMedia1(int siteNum, int deviceNum, string ParamName, ref ParamResourceType1 Info)
    {
        _ParamResourceType1 _prt1 = new _ParamResourceType1();
        bool ret = _GetParamMedia1(siteNum,deviceNum,ParamName,ref _prt1);
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        Info.fileId = _prt1.fileId;
        Info.folderId = _prt1.folderId;
        Info.path = enc.GetString(_prt1.path).TrimEnd(new char[] { '\0' });
        Info.projectPath = enc.GetString(_prt1.projectPath).TrimEnd(new char[] { '\0' });
        return ret;
    }


    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetParamObject1")]
    private static extern bool _GetParamObject1(int siteNum, int deviceNum, string ParamName, ref _ParamResourceType1 Info);

    /// <summary>
    /// Get information of a specific parameter
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="ParamName">Name for the parameter. (see Class Param)</param>
    /// <param name="Info">ParamResourceType1 object to write information to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    public static bool GetParamObject1(int siteNum, int deviceNum, string ParamName, ref ParamResourceType1 Info)
    {
        _ParamResourceType1 _prt1 = new _ParamResourceType1();
        bool ret = _GetParamObject1(siteNum,deviceNum,ParamName,ref _prt1);
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        Info.fileId = _prt1.fileId;
        Info.folderId = _prt1.folderId;
        Info.path = enc.GetString(_prt1.path).TrimEnd(new char[] { '\0' });
        Info.projectPath = enc.GetString(_prt1.projectPath).TrimEnd(new char[] { '\0' });
        return ret;
    }


    /// <summary>
    /// Get the transport mode for a specific media
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <param name="TransportMode">TransportMode object to write result to</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks></remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetMediaTransportMode")]
    public static extern bool GetMediaTransportMode(int siteNum, int deviceNum, ref TransportMode TransportMode);
    
    /// <summary>
    /// Checks wether given site is connected
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoIsSiteConnected")]
    public static extern bool IsSiteConnected(int siteNum);
    
    /// <summary>
    /// Move given layer up by one in the device tree
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveLayerUp")]
    public static extern bool MoveLayerUp(int siteNum, int deviceNum);
    
    /// <summary>
    /// Move given layer down by one in the device tree
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveLayerDown")]
    public static extern bool MoveLayerDown(int siteNum, int deviceNum);
    
    /// <summary>
    /// Move given layer to the first position in the device tree
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveLayerToFirstPosition")]
    public static extern bool MoveLayerToFirstPosition(int siteNum, int deviceNum);
    
    /// <summary>
    /// Move given layer to the last position in the device tree
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="deviceNum">Device ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoMoveLayerToLastPosition")]
    public static extern bool MoveLayerToLastPosition(int siteNum, int deviceNum);
    
    /// <summary>
    /// Enable/Disable the JogShuttle
    /// </summary>
    /// <param name="enable">Enable?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetEnableClxJogShuttle")]
    public static extern bool SetEnableClxJogShuttle(bool enable);
    
    /// <summary>
    /// Get wether the JogShuttle is enabled
    /// </summary>
    /// <returns>JogShuttle enabled?</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetEnableClxJogShuttle")]
    public static extern bool GetEnableClxJogShuttle();
    
    /// <summary>
    /// Enable/Disable the FaderExtention
    /// </summary>
    /// <param name="enable">Enable?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetEnableClxFaderExt")]
    public static extern bool SetEnableClxFaderExt(bool enable);
    
    /// <summary>
    /// Gets wether the FaderExtention is enabled
    /// </summary>
    /// <returns>FaderExtention enabled?</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetEnableClxFaderExt")]
    public static extern bool GetEnableClxFaderExt();
    
    /// <summary>
    /// Set the wait time for given cue
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceCueWaitTime")]
    public static extern bool SetSequenceCueWaitTime(int seqNum, int cueId, int hours, int minutes, int seconds, int frames);
    
    /// <summary>
    /// Set the jump target for given cue
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <param name="hours">Time (Hours)</param>
    /// <param name="minutes">Time (Minutes)</param>
    /// <param name="seconds">Time (Seconds)</param>
    /// <param name="frames">Time (Frames)</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceCueJumpTargetTime")]
    public static extern bool SetSequenceCueJumpTargetTime(int seqNum, int cueId, int hours, int minutes, int seconds, int frames);
    
    /// <summary>
    /// Set the jump count for given cue
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <param name="jumpCount">Number of jumps</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetSequenceCueJumpCount")]
    public static extern bool SetSequenceCueJumpCount(int seqNum, int cueId, int jumpCount);
    
    /// <summary>
    /// Reset the trigger count for given cue
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <param name="cueId">Cue ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoResetSequenceCueTriggerCount")]
    public static extern bool ResetSequenceCueTriggerCount(int seqNum, int cueId);
    
    /// <summary>
    /// Get wether given content is consistent
    /// </summary>
    /// <param name="dmxFolderId">DmxID (Folder)</param>
    /// <param name="dmxId">DmxID (Item)</param>
    /// <returns>Is content consistent?</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetContentIsConsistent")]
    public static extern bool GetContentIsConsistent(int dmxFolderId, int dmxId);
    
    /// <summary>
    /// Get wether content identified by name is consistent
    /// </summary>
    /// <param name="ProjectPath">Path to project item</param>
    /// <returns>Is content consistent?</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetContentIsConsistentByName")]
    public static extern bool GetContentIsConsistentByName(string ProjectPath);
    
    /// <summary>
    /// Create a new sequence
    /// </summary>
    /// <returns>Sequence ID</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoCreateSequence")]
    public static extern bool CreateSequence();
    
    /// <summary>
    /// Remove sequence
    /// </summary>
    /// <param name="seqNum">Sequence ID</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoRemoveSequence")]
    public static extern bool RemoveSequence(int seqNum);

    /// <summary>
    /// Enable/Disable cursor in fullscreen
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="showCursor">Show cursor?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetShowCursorInFullscreen")]
    public static extern bool SetShowCursorInFullscreen(int siteNum, bool showCursor);
    
    /// <summary>
    /// Set site as audio clock master
    /// </summary>
    /// <param name="siteNum">Site ID</param>
    /// <param name="isMaster">Is site master?</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetNodeOfSiteIsAudioClockMaster")]
    public static extern bool SetNodeOfSiteIsAudioClockMaster(int siteNum, bool isMaster);
    
    /// <summary>
    /// Get thumbnail for given path
    /// </summary>
    /// <param name="ProjectPath">Path to project item</param>
    /// <param name="Width">Integer to write width to</param>
    /// <param name="Height">Integer to write height to</param>
    /// <param name="Data">Image Data</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetThumbnailByPath")]
    public static extern bool GetThumbnailByPath(string ProjectPath, ref int Width, ref int Height, ref object Data);
    
    /// <summary>
    /// Get thumbnail for given item index
    /// </summary>
    /// <param name="treeItemIndex">Item index</param>
    /// <param name="Width">Integer to write width to</param>
    /// <param name="Height">Integer to write height to</param>
    /// <param name="Data">Image Data</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetThumbnailByItemIndex")]
    public static extern bool GetThumbnailByItemIndex(int treeItemIndex, ref int Width, ref int Height, ref object Data);
    
    /// <summary>
    /// Add encryption key
    /// </summary>
    /// <param name="Key">The encrption key</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddEncryptionKey")]
    public static extern bool AddEncryptionKey(string Key);
    
    /// <summary>
    /// Add encryption policy
    /// </summary>
    /// <param name="Policy">The encryption policy</param>
    /// <returns>Success. When false is returned check AutoGetLastError()</returns>
    /// <remarks>-</remarks>
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddEncryptionPolicy")]
    public static extern bool AddEncryptionPolicy(string Policy);
    
    [Obsolete("This function exists for legacy support. Please use AutoSetParamDouble/AutoSetParamByte etc.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParam")]
    public static extern bool SetParam(int siteNum, int deviceNum, string ParamName, int value);
    
    [Obsolete("This function exists for legacy support. Please use AutoSetParamDoubleExtended etc.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamExtended")]
    public static extern bool SetParamExtended(int siteNum, int deviceNum, string ParamName, int value, bool silent, bool direct);
    
    [Obsolete("This function exists for legacy support. Please use AutoSetParamDoubleInSelection etc.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamInSelection")]
    public static extern bool SetParamInSelection(string ParamName, int value);
    
    [Obsolete("This function exists for legacy support. Please use AutoSetParamRelativeDouble etc.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamRelative")]
    public static extern bool SetParamRelative(int siteNum, int deviceNum, string ParamName, int value);
    
    [Obsolete("This function exists for legacy support. Please use AutoSetParamRelativeDoubleInSelection etc.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoSetParamRelativeInSelection")]
    public static extern bool SetParamRelativeInSelection(string ParamName, int value);
    
    [Obsolete("This function exists for legacy support. Use AutoGetMediaInfo1 instead.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetMediaInfo")]
    public static extern bool GetMediaInfo(int index, ref MediaType MediaInfo);
    
    [Obsolete("This function exists for legacy support.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetParamMedia")]
    public static extern bool GetParamMedia(int siteNum, int deviceNum, string ParamName, ref ParamResourceType Info);
    
    [Obsolete("This function exists for legacy support.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoGetParamObject")]
    public static extern bool GetParamObject(int siteNum, int deviceNum, string ParamName, ref ParamResourceType Info);
    
    [Obsolete("This function exists for legacy support.")]
    [DllImport("PandorasAutomation.dll", EntryPoint = "AutoAddMediaIncrementID")]
    public static extern bool AddMediaIncrementID(string MediaPath, int siteNum, ref ParamResourceType Info);

}