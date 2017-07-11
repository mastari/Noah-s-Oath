using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class SaveFile {

    public HealthData HealthData;
    public LocationData LocationData;

    public static void Save(SaveFile s, string path) {
        var serializer = new XmlSerializer(typeof(SaveFile));
        var stream = new FileStream(Application.persistentDataPath+"/"+path, FileMode.Create);
        serializer.Serialize(stream, s);
        stream.Close();
    }

    public static SaveFile Load(string path) {
        var serializer = new XmlSerializer(typeof(SaveFile));
        var stream = new FileStream(Application.persistentDataPath+"/"+path, FileMode.Open);
        var container = serializer.Deserialize(stream) as SaveFile;
        stream.Close();
        return container;
    }

}

public class HealthData {
    public float Health;
}

public class LocationData {
    public Vector3 Location;
}
