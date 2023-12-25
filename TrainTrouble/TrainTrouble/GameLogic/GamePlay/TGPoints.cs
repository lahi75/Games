using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGGameLibrary
{
    class TGPoints
    {
        public Int32 PickedupPassengers
        {
            get
            {
                return _pickedUp;
            }
            set
            {
                _pickedUp = value;
            }
        }

        public void AddPassenger(double passengerValue)
        {
            _points += (Int32)(_pointsPerPassenger * passengerValue);
        }        

        public Int32 Points
        {
            get
            {
                return _points;
            }
            set
            {                
                _points = value;
                if (_points < 0)
                    _points = 0;
            }
        }

        public Int32 Stops
        {
            get
            {
                return _stops;
            }
            set
            {
                _stops = value;
            }
        }

        public Int32 Boosts
        {
            get
            {
                return _boost;
            }
            set
            {
                _boost = value;
            }
        }

        const Int32 _pointsPerPassenger = 10;

        Int32 _points = 0;
        Int32 _pickedUp = 0;
        Int32 _stops = 3;
        Int32 _boost = 3;
    }
}
