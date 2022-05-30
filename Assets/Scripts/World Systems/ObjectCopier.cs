using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class ObjectCopier
{
    public static T DeepCopy<T>(this T self)
    {
        var serialized = JsonConvert.SerializeObject(self);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
