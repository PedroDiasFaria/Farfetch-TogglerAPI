using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class FeatureController : ApiController
    {
        private WebAPIContext db = new WebAPIContext();

        // GET: api/Feature
        public HttpResponseMessage GetFeatures()
        {
            List<Feature> featureList = new List<Feature>();
            
            foreach (var feature in db.Features)
            {
                Feature featureModel = new Feature(feature.Name, feature.FeatureType, feature.FeatureFlag, feature.FeaturePermissions);
                featureModel.Id = feature.Id;
                featureList.Add(featureModel);
            }
            IEnumerable<Feature> features = featureList;

            return Request.CreateResponse(HttpStatusCode.OK, features);
        }

        // GET: api/Feature/5
        [ResponseType(typeof(Feature))]
        public IHttpActionResult GetFeature(long id)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }

            Feature featureModel = new Feature(feature.Name, feature.FeatureType, feature.FeatureFlag, feature.FeaturePermissions);
            featureModel.Id = feature.Id;

            return Ok(featureModel);
        }

        /// <summary>
        /// Turns On/Off the Feature's toggle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Feature/ToggleFeature/5
        [Route("api/Feature/ToggleFeature/{id}")]
        [HttpPut]
        [ResponseType(typeof(Feature))]
        public IHttpActionResult ToggleFeature(long id)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }
            

            //Create the subclass
            Feature featureModel = CreateFeature(feature, false);

            //Toggles accordingly the subclass permissions
            featureModel.Toggle();

            db.Entry(feature).State = EntityState.Detached;
            db.Entry(featureModel).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        /// <summary>
        /// Giving a type of user, gets his available features
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        // GET: api/Feature/GetUserFeatures/admin
        [Route("api/Feature/GetUserFeatures/{userType}")]
        [HttpGet]
        [ResponseType(typeof(Feature))]
        public HttpResponseMessage GetUserFeatures(string userType)
        {
            List<Feature> featureList = new List<Feature>();

            //Get only if hasPermission -> use identifier/typeUSer
            foreach (var feature in db.Features)
            {
                Feature featureModel = new Feature(feature.Name, feature.FeatureType, feature.FeatureFlag, feature.FeaturePermissions);

                if (featureModel.HasPermision(userType))
                {
                    featureModel.Id = feature.Id;
                    featureList.Add(featureModel);
                }
            }
            IEnumerable<Feature> features = featureList;

            return Request.CreateResponse(HttpStatusCode.OK, features);
        }

        /// <summary>
        /// Changes Feature's properties (such as name or type)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        // PUT: api/Feature/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeature(long id, Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feature.Id)
            {
                return BadRequest();
            }

            //Returns null if FeatureType does not exist
            Feature featureModel = CreateFeature(feature, true);

            if (featureModel == null)
            {
                return BadRequest("Invalid Feature Type");
            }

            db.Entry(feature).State = EntityState.Detached;
            db.Entry(featureModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates a new feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        // POST: api/Feature
        [ResponseType(typeof(Feature))]
        public IHttpActionResult PostFeature(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Returns null if FeatureType does not exist
            feature = CreateFeature(feature, true);

            if(feature == null)
            {
                return BadRequest("Invalid Feature Type");
            }

            db.Features.Add(feature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feature.Id }, feature);
        }

        /// <summary>
        /// Deleates a feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Feature/5
        [ResponseType(typeof(Feature))]
        public IHttpActionResult DeleteFeature(long id)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }

            db.Features.Remove(feature);
            db.SaveChanges();

            return Ok(feature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeatureExists(long id)
        {
            return db.Features.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Aux function to instantiate each Feature subclass
        /// Fixes the "Web API error: The 'ObjectContent`1' type failed to serialize" Error
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        private Feature CreateFeature(Feature feature, bool isNew)
        {
            Feature featuremodel = new Feature();

            switch (feature.FeatureType)
            {
                case "BlueButton":
                    if (isNew)
                    {
                        featuremodel = new BlueButton(feature.Name);
                    }
                    else
                    {
                        featuremodel = new BlueButton(feature.Name, feature.FeatureFlag, feature.FeaturePermissions);
                    }                    
                    break;
                case "RedButton":
                    if (isNew)
                    {
                        featuremodel = new RedButton(feature.Name);
                    }
                    else
                    {
                        featuremodel = new RedButton(feature.Name, feature.FeatureFlag, feature.FeaturePermissions);
                    }
                    break;
                case "GreenButton":
                    if (isNew)
                    {
                        featuremodel = new GreenButton(feature.Name);
                    }
                    else
                    {
                        featuremodel = new GreenButton(feature.Name, feature.FeatureFlag, feature.FeaturePermissions);
                    }
                    break;
                default:
                    return null;
            }
            featuremodel.Id = feature.Id;

            return featuremodel;
        }
    }
}