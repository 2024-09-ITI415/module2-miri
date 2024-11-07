using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;
    private Transform heroTransform; // Reference to the hero’s transform

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;

    public float maxDistance = Mathf.Infinity; // Default to no limit

    // This public property masks the field _type and takes action when it is set
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();

        // Find the hero’s transform at runtime
        GameObject hero = GameObject.FindWithTag("Hero"); 
        if (hero != null)
        {
            heroTransform = hero.transform;
        }
    }

    private void Update()
    {
        // Destroy the projectile if it goes out of bounds
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
            return;
        }

        if (_type == WeaponType.spread && heroTransform != null)
        {
            float distanceFromHero = Vector3.Distance(heroTransform.position, transform.position);

            if (distanceFromHero >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    ///<summary>
    /// Sets the _type private field and colors this projectile to match the
    /// WeaponDefinition.
    /// </summary>
    /// <param name="eType">The WeaponType to use.</param>
    public void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
