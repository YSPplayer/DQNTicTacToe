from dataManage import DataManage
from dqnManage import DQNManage
if __name__ == "__main__":
    DQNManage.get_dqn_model_message("dqn_model.h5")
    #samples = DataManage.search_data()
    #DQNManage.test_mode(samples[105].s1,samples[105].action_count)
    #DQNManage.test_mode(samples[105].s1, samples[105].action, samples[105].value)
    #DQNManage.start_train(samples,"dqn_model.h5",5)
    #DQNManage.continue_training(samples,"dqn_model.h5",500)
