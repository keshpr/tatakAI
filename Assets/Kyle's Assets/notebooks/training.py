from gym_unity.envs import UnityEnv
import numpy as np
from stable_baselines import PPO2
from stable_baselines.common.policies import MlpPolicy
from stable_baselines.common.evaluation import evaluate_policy
import gym

# wraps the unity env, returning observations and actions of training agent
# uses model to control non training agent
# trains the first agent
class MulitAgentEnvWrapper(gym.Env):

    def __init__(self, MultiAgentEnv, model=None):
        super(MulitAgentEnvWrapper, self).__init__()
        self.model = model
        self.env = MultiAgentEnv

        # define IO spaces
        self.action_space = MultiAgentEnv.action_space
        self.observation_space = MultiAgentEnv.observation_space

        # reset to make sure there is an observation for the loaded model
        self.reset()

    def reset(self):
        observations = self.env.reset()
        self.last_static_observation = observations[1]
        return observations[0]

    def step(self, train_action):
        # create actions for the env
        if self.model == None:
            static_action = self.env.action_space.sample()
        else:
            static_action = self.model.predict(self.last_static_observation)
        actions = [train_action, static_action]

        # execute one step
        observation_array, reward_array, done_array, info = self.env.step(actions)
        self.last_static_observation = observation_array[1]
        return observation_array[0], reward_array[0], done_array[0], info

    def render(self):
        pass

    def close(self):
        return self.env.close()


def main():
    # point to the binary with one brain with two agents
    FILENAME = "../../../../binaries/multiAgent10x.app"

    # start the unity environment
    worker_id = 0
    use_visual = False
    uint8_visual = False
    multiagent = True
    allow_multiple_visual_obs = True

    MultiAgentEnv = UnityEnv(FILENAME, worker_id, use_visual, uint8_visual, multiagent, allow_multiple_visual_obs)
    SingleEnv = MulitAgentEnvWrapper(MultiAgentEnv)
    print(SingleEnv.action_space)
    print(SingleEnv.observation_space)

    # create the model
    model = PPO2(MlpPolicy, SingleEnv, verbose=1)

    # evaluate the model
    '''
    mean_reward, std_reward = evaluate_policy(model, SingleEnv, n_eval_episodes=100)
    print(f"mean_reward:{mean_reward:.2f} +/- {std_reward:.2f}")
    '''

    # train the model against a random agent
    model.learn(total_timesteps=10000)

    # sleep(10)
    SingleEnv.close()

if __name__ == "__main__":
    main()