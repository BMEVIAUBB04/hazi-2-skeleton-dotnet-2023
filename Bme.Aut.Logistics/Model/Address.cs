using System;

namespace Bme.Aut.Logistics.Model;

public class Address : IEquatable<Address>
{
    // required by EF
    public Address()
    {
    }

    public Address(double geoLat, double geoLng, string country, string city, string zipCode, string street, string number)
    {
        this.GeoLatitude = geoLat;
        this.GeoLongitude = geoLng;
        this.Country = country;
        this.City = city;
        this.ZipCode = zipCode;
        this.Street = street;
        this.Number = number;
    }

    public long Id { get; set; }
    public double GeoLongitude { get; set; }
    public double GeoLatitude { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }

    #region Equals and GetHashCode

    public override bool Equals(object obj)
    {
        return Equals(obj as Address);
    }

    public bool Equals(Address other)
    {
        return other != null &&
               Id == other.Id &&
               GeoLongitude == other.GeoLongitude &&
               GeoLatitude == other.GeoLatitude &&
               Country == other.Country &&
               City == other.City &&
               ZipCode == other.ZipCode &&
               Street == other.Street &&
               Number == other.Number;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, GeoLongitude, GeoLatitude, Country, City, ZipCode, Street, Number);
    }

    #endregion
}
