using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF
{
    public class DBQuestionRepository
    {
        public int POOL_ID { get; set; }
        public string TOPIC { get; set; }
        public string SUBTOPIC { get; set; }
        public string QUESTION_TEXT { get; set; }
        public string LATEX_TEXT { get; set; }
        public string CORRECT_ANSWER { get; set; }
        public List<string> WRONG_ANSWERS { get; set; } = new List<string>();

        public DBQuestionRepository(string topic, string subtopic)
        {
            Genel.Handle(() =>
            {

                int poolId = IDRandomizer(topic, subtopic);
                var poolEntity = Genel.dbEntities.QUESTION_POOL.Find(poolId);

                // proplara veri atma
                POOL_ID = poolId;
                TOPIC = poolEntity.TOPICS.TOPIC;
                SUBTOPIC = poolEntity.SUBTOPICS.SUBTOPIC;
                QUESTION_TEXT = poolEntity.QUESTION_TEXT;
                LATEX_TEXT = poolEntity.LATEX_TEXT;
                CORRECT_ANSWER = poolEntity.CORRECT_ANSWER;

                string[] splitString = poolEntity.WRONG_ANSWERS.Split('#');
                foreach (string item in splitString)
                {
                    WRONG_ANSWERS.Add(item);
                }
            });
        }

        private int IDRandomizer(string topic, string subtopic)
        {
            List<int> ids = Genel.dbEntities.QUESTION_POOL.Where(x => x.TOPICS.TOPIC == topic && x.SUBTOPICS.SUBTOPIC == subtopic).Select(x => x.POOL_ID).ToList();

            Random rnd = new Random();
            int randomSayi = rnd.Next(ids.Count);
            return ids[randomSayi];
        }
    }
}
