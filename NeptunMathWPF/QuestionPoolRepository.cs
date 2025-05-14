using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NeptunMathWPF
{
    class QuestionPoolRepository
    {
        public int POOL_ID { get; set; }
        public int TOPIC_ID { get; set; }
        public int SUBTOPIC_ID { get; set; }
        public string TOPIC { get; set; }
        public string SUBTOPIC { get; set; }
        public string QUESTION_TEXT { get; set; }
        public string LATEX_TEXT { get; set; }
        public string CORRECT_ANSWER { get; set; }
        public string WRONG_ANSWERS { get; set; }

        public QuestionPoolRepository()
        {
            Genel.Handle(() =>
            {
                var count = Genel.dbEntities.QUESTION_POOL.Count();
                if (count>0)
                {
                    var randomIndex = new Random().Next(0, count);

                    var randomEntity = Genel.dbEntities.QUESTION_POOL.OrderBy(x=>x.POOL_ID).Skip(randomIndex).FirstOrDefault();

                    string topic = Genel.dbEntities.TOPICS.Where(x => x.TOPIC_ID == randomEntity.TOPIC_ID).Select(x => x.TOPIC).FirstOrDefault();
                    string subtopic = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC_ID == randomEntity.SUBTOPIC_ID).Select(x => x.SUBTOPIC).FirstOrDefault();

                    POOL_ID = randomEntity.POOL_ID;
                    TOPIC_ID = randomEntity.TOPIC_ID;
                    SUBTOPIC_ID = randomEntity.SUBTOPIC_ID;
                    TOPIC = topic;
                    SUBTOPIC = subtopic;
                    QUESTION_TEXT = randomEntity.QUESTION_TEXT;
                    LATEX_TEXT = randomEntity.LATEX_TEXT;
                    CORRECT_ANSWER = randomEntity.CORRECT_ANSWER;
                    WRONG_ANSWERS = randomEntity.WRONG_ANSWERS;
                }
            });
        }
    }
}
