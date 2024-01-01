from dataManage import DataManage
from dqnManage import DQNManage
if __name__ == "__main__":
    samples = DataManage.search_data()
    DQNManage.test_mode(samples[105].s1,samples[105].action_count)
    #DQNManage.test_mode(samples[105].s1, samples[105].action, samples[105].value)
   # DQNManage.start_train(samples)
