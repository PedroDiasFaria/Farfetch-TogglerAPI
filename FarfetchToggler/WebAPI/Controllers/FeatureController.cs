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

        // GET: api/Feature/ToggleFeature/5
        [Route("api/Feature/ToggleFeature/{id}")]
        [HttpGet]
        [ResponseType(typeof(Feature))]
        public IHttpActionResult ToggleFeature(long id)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }
            db.Entry(feature).State = EntityState.Detached;

            Feature featuremodel = createFeature(feature);

            featuremodel.Toggle();

            db.Entry(featuremodel).State = EntityState.Modified;
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

            db.Entry(feature).State = EntityState.Modified;

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

        // POST: api/Feature
        [ResponseType(typeof(Feature))]
        public IHttpActionResult PostFeature(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Features.Add(feature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feature.Id }, feature);
        }

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

        private Feature createFeature(Feature feature)
        {
            Feature featuremodel = new Feature();

            switch (feature.FeatureType)
            {
                case "BlueButton":
                    featuremodel = new BlueButton(feature.Name, feature.FeatureFlag, feature.FeaturePermissions);
                    break;
                case "RedButton":
                    featuremodel = new RedButton(feature.Name, feature.FeatureFlag, feature.FeaturePermissions);
                    break;
                case "GreenButton":
                    featuremodel = new GreenButton(feature.Name, feature.FeatureFlag, feature.FeaturePermissions);
                    break;
                default:
                    return null;
            }

            featuremodel.Id = feature.Id;

            return featuremodel;

        }
    }
}