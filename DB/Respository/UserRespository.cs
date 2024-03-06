using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HexStrategyInRazor.Map.DB.Interfaces;
using HexStrategyInRazor.DB;
using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.Map.DB.Respository
{
    public class UserRespository : AbstractRepository<User>, IUserRepository
    {
        public UserRespository(WadbContext context) : base(context)
        {

        }

        public string AddWithErrorText(User entity, out bool foundError)
        {
            entity.Password = Encryption.Encrypt(entity.Password);
            entity.IsDecripted = false;
            dbSet.Add(entity);

            foundError = false;
            return "";
        }

        public void Add(User entity)
        {
            entity.Password = Encryption.Encrypt(entity.Password);
            entity.IsDecripted = false;

            dbSet.Add(entity);

            context.SaveChanges();
        }

        public void Delete(User entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var toRemove = dbSet.Find(id);

            if (toRemove != null)
            {
                dbSet.Remove(toRemove);
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            List<User> toReturn = new(dbSet);
            Parallel.ForEach(toReturn, entity =>
            {
                if (!entity.IsDecripted)
                {
                    entity.Password = Encryption.Decrypt(entity.Password);
                    entity.IsDecripted = true;
                }
            });
            return toReturn;
        }

        public User? GetById(int id)
        {
            var user = dbSet.Find(id);
            if (user != null)
                user.Password = Encryption.Decrypt(user.Password);
            return user;
        }

        public void Update(User entity)
        {
            entity.Password = Encryption.Encrypt(entity.Password);
            entity.IsDecripted = false;
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
