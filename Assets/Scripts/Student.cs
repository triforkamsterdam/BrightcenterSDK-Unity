using UnityEngine;
using System.Collections;

public class Student {

    public string id { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }

    public Student(string id, string firstName, string lastName) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
    }
}
