using System.Collections.Generic;
using System.IO;
using Script;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    public GameObject CubePrefab;

    private List<GameObject> _cubeList = new List<GameObject>();
    private string _savedData;
    string _filePath = Directory.GetCurrentDirectory() + @"\Assets\DataIG\SaveFile.json";
    private void Update() {
        if (Input.GetButton("Jump")) {
            GameObject instantiate = Instantiate(CubePrefab, transform.position, Quaternion.identity);
            _cubeList.Add(instantiate);
        }
        if (Input.GetButtonDown("Fire1")) {
            // Serialization process
            FileSave fileSave = new FileSave(_cubeList);
            _savedData = JsonUtility.ToJson(fileSave);
            
            //fichier
            
            using (StreamWriter writer = new StreamWriter(_filePath))  
            {  
                writer.WriteLine(_savedData);
            }  
        }
        if (Input.GetButtonDown("Fire2")) {
            // Clear state
            foreach (GameObject o in _cubeList) {
                Destroy(o);
            }
            _cubeList.Clear();
            // Deserialization process
            string readText;
            using(StreamReader readtext = new StreamReader(_filePath))
            {
                readText = readtext.ReadLine();
            }
            FileSave fileSave = JsonUtility.FromJson<FileSave>(readText);
            foreach (SerializableTransform serializableTransform in fileSave.SerializableTransforms) {
                GameObject instantiate = Instantiate(CubePrefab, transform.position, Quaternion.identity);
                instantiate.transform.position = serializableTransform.Position;
                instantiate.transform.rotation = serializableTransform.Rotation;
                instantiate.transform.localScale = serializableTransform.Scale;
                _cubeList.Add(instantiate);
            }
        }
    }

}
