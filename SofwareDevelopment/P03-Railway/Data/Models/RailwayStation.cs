﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_Railway.Data.Models
{
    public class RailwayStation
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }
        public virtual ICollection<TrainRailwayStation> TrainsRailwayStations { get; set; } = new List<TrainRailwayStation>();

    }
}
