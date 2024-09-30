import { SyntheticEvent, useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import {
  Tab,
  Grid,
  Header,
  Card,
  Image,
  TabProps,
  TabPane,
  Button,
  Loader,
  Segment,
} from "semantic-ui-react";
import { Link } from "react-router-dom";
import { format } from "date-fns";
import { useStore } from "../../app/stores/store";
import { UserActivity } from "../../app/models/profile";
import { PagingParams } from "../../app/models/pagination";
import InfiniteScroll from "react-infinite-scroller";
const panes = [
  { menuItem: "Future Events", pane: { key: "future" } },
  { menuItem: "Past Events", pane: { key: "past" } },
  { menuItem: "Hosting", pane: { key: "hosting" } },
];
export default observer(function ProfileActivities() {
  const { profileStore } = useStore();
  const {
    loadUserActivities,
    profile,
    loadingActivities,
    userActivities,
    setPagingParams,
    pagination,
    resetUserActivities,
  } = profileStore;
  const [loadingNext, setLoadingNext] = useState(false);
  const [tabData, setTabData] = useState("");

  useEffect(() => {
    loadUserActivities(profile!.username, "future");
  }, [loadUserActivities, profile, tabData]);

  const handleTabChange = (_: SyntheticEvent, data: TabProps) => {
    setPagingParams(new PagingParams());
    resetUserActivities();
    const newTabData = panes[data.activeIndex as number].pane.key;
    setTabData(newTabData);
    loadUserActivities(profile!.username, newTabData);
  };

  function handleGetNext() {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadUserActivities(profile!.username, tabData).then(() =>
      setLoadingNext(false)
    );
  }

  return (
    <TabPane loading={loadingActivities}>
      <Grid>
        <Grid.Column width={16}>
          <Header floated="left" icon="calendar" content={"Activities"} />
        </Grid.Column>
        <Grid.Column width={16}>
          <Tab
            panes={panes}
            menu={{ secondary: true, pointing: true }}
            onTabChange={(e, data) => handleTabChange(e, data)}
          />
          <br />
          <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={
              !loadingNext &&
              !!pagination &&
              pagination.currentPage < pagination.totalPages
            }
            initialLoad={false}
          >
            <Card.Group itemsPerRow={4}>
              {userActivities.map((activity: UserActivity) => (
                <Card
                  as={Link}
                  to={`/activities/${activity.id}`}
                  key={activity.id}
                >
                  <Image
                    src={`/assets/categoryImages/${activity.category}.jpg`}
                    style={{ minHeight: 100, objectFit: "cover" }}
                  />
                  <Card.Content>
                    <Card.Header textAlign="center">
                      {activity.title}
                    </Card.Header>
                    <Card.Meta textAlign="center">
                      <div>{format(new Date(activity.date), "do LLL")}</div>
                      <div>{format(new Date(activity.date), "h:mm a")}</div>
                    </Card.Meta>
                  </Card.Content>
                </Card>
              ))}
            </Card.Group>
            <Grid.Column width={10}>
              <Loader active={loadingNext} />
            </Grid.Column>
          </InfiniteScroll>
        </Grid.Column>
      </Grid>
      <Grid centered>
        <Grid.Column width={16}>
          <Segment
            attached="top"
            compact
            textAlign="center"
            basic
            size="mini"
            content="You can scroll or click the button to load more if there are..."
          />
          <Button
            style={{ marginLeft: "50%" }}
            loading={loadingActivities}
            disabled={
              loadingActivities ||
              (!!pagination &&
                pagination.totalPages === pagination.currentPage) ||
              (!!pagination && pagination.totalPages < pagination.currentPage)
            }
            onClick={handleGetNext}
            content="More"
            size="mini"
            positive
            floated="left"
          ></Button>
        </Grid.Column>
      </Grid>
    </TabPane>
  );
});
