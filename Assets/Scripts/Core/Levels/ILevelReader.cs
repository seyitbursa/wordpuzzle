using System;
using System.Collections.Generic;
using Core.Levels;
using UnityEngine;

namespace Core.Levels
{
    public interface ILevelReader
    {
        List<LevelDetail> ReadLevels(string input);
    }
}