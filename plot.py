import pandas as pd
import matplotlib.pyplot as plt

mmp1 = pd.read_csv('testsizemmp.csv')
ms1  = pd.read_csv('testsizems.csv')
mmp1 = mmp1.values
ms1  = ms1.values

mmp2 = pd.read_csv('testrangemmp.csv')
ms2  = pd.read_csv('testrangems.csv')
mmp2 = mmp2.values
ms2  = ms2.values

mmp3 = pd.read_csv('testrangebiggermmp.csv')
ms3  = pd.read_csv('testrangebiggerms.csv')
mmp3 = mmp3.values
ms3  = ms3.values

pows = [2 ** i for i in range(7, 21)]
pows.remove(524288)
pows.remove(32768)

fig, [ax1, ax2, ax3] = plt.subplots(1, 3, sharey=False)

ax1.set_xscale('log')
ax1.grid()
ax1.set_xlabel('Input Size')
ax1.set_ylabel('Time Taken (ms)')

ax1.plot(mmp1[:, 0], mmp1[:, 1], label="Multiply Mod Prime")
ax1.plot(ms1[:, 0], ms1[:, 1], label="Multiply Shift")
ax1.legend()

ax2.set_xscale('log')
ax2.set_yscale('log')
ax2.grid()
ax2.set_xlabel('id range (1-n)')

ax2.plot(pows, mmp2[:, 1], label="Multiply Mod Prime")
ax2.plot(pows, ms2[:, 1], label="Multiply Shift")
ax2.legend()

ax3.set_xscale('log')
ax3.set_yscale('log')
ax3.grid()
ax3.set_xlabel('id range (1-n)')

ax3.plot(pows, mmp3[:, 1], label="Multiply Mod Prime")
ax3.plot(pows, ms3[:, 1], label="Multiply Shift")
ax3.legend()

plt.show()
